using Eventing.Core.Domain;
using Eventing.Core.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventing.Core.Persistence
{
    public class InMemoryEventSourcedRepository : IEventSourcedRepository
    {
        private readonly object lockObject = new object();

        protected IJsonSerializer serializer = new NewtonsoftJsonSerializer();
        protected Dictionary<string, List<string>> streams = new Dictionary<string, List<string>>();
        protected Dictionary<string, string> snapshots = new Dictionary<string, string>();

        public async Task SaveAsync(IEventSourced eventSourced)
        {
            Ensure.NotNullOrWhiteSpace(eventSourced.StreamName, nameof(eventSourced.StreamName));

            lock (this.lockObject)
            {
                if (!this.streams.ContainsKey(eventSourced.StreamName))
                    this.streams.Add(eventSourced.StreamName, new List<string>());

                this.streams[eventSourced.StreamName].AddRange(eventSourced.Dehydrate().Select(e => this.serializer.Serialize(e)));
                this.snapshots[eventSourced.StreamName] = this.serializer.Serialize(eventSourced.TakeSnapshot());
            }
            await Task.CompletedTask;
        }

        public async Task<T> GetByIdAsync<T>(string streamId) where T : class, IEventSourced, new()
        {
            var streamName = StreamCategoryAttribute.GetFullStreamName<T>(streamId);
            return await this.GetAsync<T>(streamName);
        }

        public async Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new()
        {
            if (!this.streams.ContainsKey(streamName))
                return null;

            var state = new T();
            lock (this.lockObject)
            {
                this.streams[streamName].ForEach(x => state.Apply(this.serializer.Deserialize(x)));
            }

            return await Task.FromResult(state);
        }
    }
}
