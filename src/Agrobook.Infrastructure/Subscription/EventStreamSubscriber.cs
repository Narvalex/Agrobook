using Agrobook.Core;
using EventStore.ClientAPI;
using System;

namespace Agrobook.Infrastructure.Subscription
{
    public class EventStreamSubscriber : IEventStreamSubscriber
    {
        private readonly IEventStoreConnection resilientConnection;
        private readonly TimeSpan closeTimeout;

        public EventStreamSubscriber(IEventStoreConnection resilientConnection)
            : this(resilientConnection, TimeSpan.FromMinutes(1))
        { }

        public EventStreamSubscriber(IEventStoreConnection resilientConnection, TimeSpan closeTimeout)
        {
            Ensure.NotNull(resilientConnection, nameof(resilientConnection));

            this.resilientConnection = resilientConnection;
            this.closeTimeout = closeTimeout;
        }

        public IEventStreamSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler)
        {
            return new EventStreamSubscription(
                this.resilientConnection,
                streamName,
                lastCheckpoint,
                handler,
                this.closeTimeout);
        }
    }
}
