using Agrobook.Core;
using EventStore.ClientAPI;
using System;

namespace Agrobook.Infrastructure.Subscription
{
    public class EventStreamSubscriber : IEventStreamSubscriber
    {
        private readonly IEventStoreConnection resilientConnection;
        private readonly IJsonSerializer serializer;

        public EventStreamSubscriber(IEventStoreConnection resilientConnection, IJsonSerializer serializer)
        {
            Ensure.NotNull(resilientConnection, nameof(resilientConnection));
            Ensure.NotNull(serializer, nameof(serializer));

            this.resilientConnection = resilientConnection;
            this.serializer = serializer;
        }

        public IEventStreamSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler)
        {
            return new EventStreamSubscription(
                this.resilientConnection,
                this.serializer,
                streamName,
                lastCheckpoint,
                handler);
        }

        //public IEventStreamSubscription CreateSubscriptionFromCategory(string category, Lazy<long?> lastCheckpoint, Action<long, object> handler)
        //{
        //    return this.CreateSubscription($"$ce-{category}", lastCheckpoint, handler);
        //}
    }
}
