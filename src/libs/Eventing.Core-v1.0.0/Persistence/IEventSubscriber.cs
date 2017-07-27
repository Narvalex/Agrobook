using System;

namespace Eventing.Core.Persistence
{
    public interface IEventSubscriber
    {
        IEventSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler);
    }
}
