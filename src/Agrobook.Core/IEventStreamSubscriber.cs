using System;

namespace Agrobook.Core
{
    public interface IEventStreamSubscriber
    {
        IEventStreamSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler);
        //IEventStreamSubscription CreateSubscriptionFromCategory(string category, Lazy<long?> lastCheckpoint, Action<long, object> handler);
    }
}
