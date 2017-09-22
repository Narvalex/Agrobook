using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Tests
{
    public class FakeEventSubscription : IEventSubscription
    {
        public void Start() { }

        public void Stop() { }
    }

    public class FakeSubscriber : IEventSubscriber
    {
        public IEventSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler)
            => new FakeEventSubscription();

        public IEventSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long> persistCheckpoint, Action<long, object> handler)
            => new FakeEventSubscription();
    }

    public class FakeCheckpointRepository : ICheckpointRepository
    {
        public long? GetCheckpoint(string subscriptionId)
        {
            throw new NotImplementedException();
        }

        public Task<long?> GetCheckpointAsync(string subscriptionId)
        {
            throw new NotImplementedException();
        }

        public void SaveCheckpoint(string subscriptionId, long checkpoint)
        {
            throw new NotImplementedException();
        }

        public Task SaveCheckpointAsync(string subscriptionId, long checkpoint)
        {
            throw new NotImplementedException();
        }
    }
}
