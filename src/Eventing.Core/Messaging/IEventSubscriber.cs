using System;

namespace Eventing.Core.Messaging
{
    public interface IEventSubscriber
    {
        /// <summary>
        /// Creates a subscription where the handler is not idempotent but the checkpoint is persisted in a transaction with 
        /// the handling. E.g.: SQL server
        /// </summary>
        IEventSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler);

        /// <summary>
        /// Creates a subscription where the handler must be idempotent since there is method that is not transactional with the handling 
        /// that perists the checkpoint. E.g: an Saga Execution Coordinator that listens to aggregate events
        /// </summary>
        IEventSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long> persistCheckpoint, Action<long, object> handler);
    }
}
