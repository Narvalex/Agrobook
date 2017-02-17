using Agrobook.Core;
using Agrobook.Infrastructure.Serialization;
using EventStore.ClientAPI;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Infrastructure.EventSourcing
{
    public class EventSourcedRepository : IEventSourcedRepository
    {
        private readonly Func<Task<IEventStoreConnection>> connectionFactory;
        private readonly IJsonSerializer serializer;

        private readonly int readPageSize;
        private readonly int writePageSize;

        public EventSourcedRepository(
            Func<Task<IEventStoreConnection>> connectionFactory,
            IJsonSerializer serializer,
            int readPageSize = 500,
            int writePageSize = 500)
        {
            Ensure.NotNull(connectionFactory, nameof(connectionFactory));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.Positive(readPageSize, nameof(readPageSize));
            Ensure.Positive(writePageSize, nameof(writePageSize));

            this.connectionFactory = connectionFactory;
            this.serializer = serializer;
            this.readPageSize = readPageSize;
            this.writePageSize = writePageSize;
        }

        public async Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new()
        {
            var state = new T();

            var connection = await this.connectionFactory.Invoke();

            var sliceStart = 0;

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
                        }
                        break;

                    case SliceReadStatus.StreamNotFound:
                        return null;

                    case SliceReadStatus.StreamDeleted:
                        break;

                    default:
                        break;
                }

            } while (true);

            throw new InvalidOperationException();

        }

        public Task SaveAsync<T>(T updatedState) where T : class, IEventSourced, new()
        {
            throw new NotImplementedException();
        }

        private object Deserialize(ResolvedEvent e)
        {
            var serialized = Encoding.UTF8.GetString(e.Event.Data);
            return this.serializer.Deserialize(serialized);
        }
    }
}
