using Eventing.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventing.Core.Messaging
{
    /// <summary>
    /// Provides basic common processing code for components that handles
    /// incoming events from a subscription.
    /// </summary>
    public class EventProcessor : IDisposable
    {
        private readonly ILogLite logger;

        private readonly IEventSubscription subscription;
        private readonly Dictionary<Type, IEventHandler> handlersByType = new Dictionary<Type, IEventHandler>();

        private bool disposed = false; // To detect redundant calls
        private bool started = false;

        private Action<Exception> exceptionHandler;

        private readonly object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventProcessor"/> class.
        /// </summary>
        public EventProcessor(IEventSubscription subscription, string processorName = null)
        {
            Ensure.NotNull(subscription, nameof(subscription));

            this.subscription = subscription;
            this.logger = LogManager.GetLoggerFor(processorName ?? "EventProcessor-" + Guid.NewGuid().ToString());
        }

        /// <summary>
        /// Registers the specified event handler.
        /// </summary>
        public void Register(IEventHandler handler)
        {
            var genericHandler = typeof(IHandler<>);
            var supportedEventTypes = handler.GetType()
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericHandler)
                .Select(i => i.GetGenericArguments()[0])
                .ToList();

            if (this.handlersByType.Keys.Any(registeredType => supportedEventTypes.Contains(registeredType)))
                throw new ArgumentException("The event handled by the received handler has a registered handler.");

            supportedEventTypes.ForEach(eventType => this.handlersByType.Add(eventType, handler));
        }

        /// <summary>
        /// Starts the listener.
        /// </summary>
        public void Start(Action<Exception> onException = null)
        {
            if (this.disposed) throw new ObjectDisposedException("EventProcessor");
            lock (this.lockObject)
            {
                if (!this.started)
                {
                    if (onException != null)
                        this.exceptionHandler = onException;
                    this.subscription.SetListener(this.OnEventApeared);
                    this.subscription.Start();
                    this.started = true;
                }
            }
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
        public void Stop()
        {
            lock (this.lockObject)
            {
                if (this.started)
                {
                    this.subscription.Stop();
                    // No need to remove the listener. The subscription will stop receiving events anyway.
                    this.started = false;
                }
            }
        }

        private async Task OnEventApeared(long checkpoint, object @event)
        {
            var eventType = @event.GetType();
            IEventHandler handler = null;

            if (this.handlersByType.TryGetValue(eventType, out handler))
            {
                try
                {
                    await ((dynamic)handler).Handle(checkpoint, (dynamic)@event);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, $"Unhanled exception in handler {handler.ToString()} when processing event of type {@event.GetType().Name} from stream {this.subscription.SubscriptionStreamName} with checkpoint {checkpoint}");
                    this.exceptionHandler.Invoke(ex);

                    throw;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //Dispose managed state(managed objects).
                    this.Stop();
                    this.disposed = true;

                    using (this.subscription as IDisposable)
                    {
                        // Dispose subscription if it's disposable.
                    }

                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~EventProcessor()
        {
            this.Dispose(false);
        }
    }
}
