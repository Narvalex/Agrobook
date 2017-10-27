using Eventing.Core.Domain;
using Eventing.Core.Messaging;
using Eventing.Core.Serialization;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eventing.GetEventStore
{
    public class EventStoreEventSourcedRepository : IEventSourcedRepository
    {
        private readonly Func<Task<IEventStoreConnection>> connectionFactory;
        private readonly IJsonSerializer serializer;
        private readonly ISnapshotCache snapshotCache;

        private readonly int readPageSize;
        private readonly int writePageSize;

        public EventStoreEventSourcedRepository(
         Func<Task<IEventStoreConnection>> connectionFactory,
            IJsonSerializer serializer,
            ISnapshotCache snapshotCache,
            int readPageSize = 500,
            int writePageSize = 500)
        {
            Ensure.NotNull(connectionFactory, nameof(connectionFactory));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(snapshotCache, nameof(snapshotCache));
            Ensure.Positive(readPageSize, nameof(readPageSize));
            Ensure.Positive(writePageSize, nameof(writePageSize));

            this.connectionFactory = connectionFactory;
            this.serializer = serializer;
            this.snapshotCache = snapshotCache;
            this.readPageSize = readPageSize;
            this.writePageSize = writePageSize;
        }

        public async Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new()
        {
            var eventSourced = new T();
            if (this.snapshotCache.TryGet(streamName, out var snapshot))
            {
                eventSourced.Rehydrate(snapshot);
                return eventSourced;
            }

            var connection = await this.connectionFactory.Invoke();

            long sliceStart = 0;

            StreamEventsSlice currentSlice;
            do
            {
                currentSlice = await connection.ReadStreamEventsForwardAsync(streamName, sliceStart, this.readPageSize, false);

                switch (currentSlice.Status)
                {
                    case SliceReadStatus.Success:
                        sliceStart = currentSlice.NextEventNumber;

                        foreach (var e in currentSlice.Events)
                        {
                            var deserialized = this.Deserialize(e);
                            eventSourced.Apply(deserialized);
                        }
                        break;

                    case SliceReadStatus.StreamNotFound:
                        return null;

                    case SliceReadStatus.StreamDeleted:
                        throw new InvalidOperationException($"The stream {streamName} was deleted");

                    default:
                        throw new NotSupportedException($"The status of type {currentSlice.Status} is not supported");
                }

            } while (!currentSlice.IsEndOfStream);

            return eventSourced;
        }

        public async Task<bool> Exists(string streamName)
        {
            if (this.snapshotCache.TryGet(streamName, out var snapshot))
                return true;

            var connection = await this.connectionFactory.Invoke();
            var readResult = await connection.ReadEventAsync(streamName, StreamPosition.Start, false);
            return readResult.Status == EventReadStatus.Success;
        }

        public async Task SaveAsync(IEventSourced eventSourced)
        {

            var newEvents = eventSourced.Dehydrate();
            if (newEvents.Count < 1)
                return;

            var connection = await this.connectionFactory.Invoke();
            var commitId = Guid.NewGuid();
            var newBuiltEvents = newEvents.Select(e => this.BuildEventData(commitId, e)).ToArray();
            var expectedVersion = eventSourced.Version - newBuiltEvents.Length;

            if (newBuiltEvents.Length <= this.writePageSize)
                await connection.AppendToStreamAsync(eventSourced.StreamName, expectedVersion, newBuiltEvents);
            else
            {
                var transaction = await connection.StartTransactionAsync(eventSourced.StreamName, expectedVersion);

                var position = 0;
                while (position < newBuiltEvents.Length)
                {
                    var pageEvents = newBuiltEvents.Skip(position).Take(this.writePageSize);
                    await transaction.WriteAsync(pageEvents);
                    position += this.writePageSize;
                }

                await transaction.CommitAsync();
            }

            this.snapshotCache.Cache(eventSourced.TakeSnapshot());
        }

        private object Deserialize(ResolvedEvent e)
        {
            var serialized = Encoding.UTF8.GetString(e.Event.Data);
            return this.serializer.Deserialize(serialized);
        }

        private EventData BuildEventData(Guid commitId, object @event)
        {
            var eventId = Guid.NewGuid();

            var eventHeaders = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("commitId", commitId.ToString()),
                new KeyValuePair<string, string>("eventId", eventId.ToString())
            };

            var metadataBytes = Encoding.UTF8.GetBytes(this.serializer.Serialize(eventHeaders));
            var dataBytes = Encoding.UTF8.GetBytes(this.serializer.Serialize(@event));

            var eventType = @event.GetType().Name.WithFirstCharInLower();

            return new EventData(eventId, eventType, true, dataBytes, metadataBytes);
        }
    }
}
