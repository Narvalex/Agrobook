using System;

namespace Eventing.Core.Messaging
{
    /// <summary>
    /// Abstracts the behavior of a receiver component that raises 
    /// and event for every received event.
    /// The Event Subscription is responsible of handling the checkpointing.
    /// </summary>
    public interface IEventSubscription
    {
        /// <summary>
        /// Registers the action to be invoked whenever a new event apears.
        /// </summary>
        /// <param name="listener">An action invoked when an event is received over the subscription</param>
        void SetListener(Action<long, object> listener);

        /// <summary>
        /// Starts the listener.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the listener.
        /// </summary>
        void Stop();
    }
}
