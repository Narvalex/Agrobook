using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using EventStore.ClientAPI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eventing.GetEventStore
{
    public class EventStoreCheckpointRepository : ICheckpointRepository
    {
        private readonly Func<Task<IEventStoreConnection>> connectionFactory;
        private readonly IJsonSerializer serializer;
        private readonly string streamPrefix = "checkpoint.repo";

        public EventStoreCheckpointRepository(Func<Task<IEventStoreConnection>> connectionFactory, IJsonSerializer serializer)
        {
            Ensure.NotNull(connectionFactory, nameof(connectionFactory));
            Ensure.NotNull(serializer, nameof(serializer));

            this.connectionFactory = connectionFactory;
            this.serializer = serializer;
        }

        public long? GetCheckpoint(string subscriptionId) => this.GetCheckpointAsync(subscriptionId).Result;

        public async Task<long?> GetCheckpointAsync(string subscriptionId)
        {
            var connection = await this.connectionFactory.Invoke();

            var readResult = await connection.ReadEventAsync(this.GetSubStreamName(subscriptionId), StreamPosition.Start, false);
            if (readResult.Status != EventReadStatus.Success)
                return null;
            var deserialized = this.Deserialize(readResult.Event.Value);
            var e = (SubscriptionCheckpoint)deserialized;
            return e.EventNumber;
        }

        public void SaveCheckpoint(string subscriptionId, long checkpoint) => this.SaveCheckpointAsync(subscriptionId, checkpoint).Wait();

        public async Task SaveCheckpointAsync(string subscriptionId, long checkpoint)
        {
            var connection = await this.connectionFactory.Invoke();

            var setMetadata = !(await this.GetCheckpointAsync(subscriptionId)).HasValue;

            var e = this.BuildEventData(Guid.NewGuid(), new SubscriptionCheckpoint(checkpoint));
            await connection.AppendToStreamAsync(this.GetSubStreamName(subscriptionId), ExpectedVersion.Any, e);

            if (setMetadata)
                await TrySetMetadata(subscriptionId, connection);
        }

        private async Task TrySetMetadata(string subId, IEventStoreConnection connection)
        {
            var result = await connection.GetStreamMetadataAsync(this.GetSubStreamName(subId));
            if (!result.StreamMetadata.MaxCount.HasValue || result.StreamMetadata.MaxCount != 1)
            {
                await connection.SetStreamMetadataAsync(this.GetSubStreamName(subId), ExpectedVersion.Any,
                    StreamMetadata.Build().SetMaxCount(1).Build());
            }
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

        private string GetSubStreamName(string subId) => $"{this.streamPrefix}-{subId}";
    }

    public class SubscriptionCheckpoint
    {
        public SubscriptionCheckpoint(long eventNumber)
        {
            this.EventNumber = eventNumber;
        }

        public long EventNumber { get; }
    }
}
