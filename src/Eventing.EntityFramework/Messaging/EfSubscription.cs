using Eventing.Core.Messaging;
using Eventing.Core.Serialization;
using Eventing.EntityFramework.Persistence;
using Eventing.Log;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Eventing.EntityFramework.Messaging
{
    public class EfSubscription : IEventSubscription, IDisposable
    {
        private ILogLite log = LogManager.GetLoggerFor<EfSubscription>();

        private readonly string subscriptionId;
        private readonly EfEventStore eventStore;
        private readonly IJsonSerializer serializer;
        private Func<long, object, Task> listener;

        private bool hasExternalCheckpointSource;
        private Func<long?> externalCheckpointSource;

        private long? currentCheckpoint;
        private long? onLiveCheckpoint;

        private readonly object lockObject = new object();
        private CancellationTokenSource cancellationSource = null;

        private Task receiveEventsTask;

        public EfSubscription(string subscriptionId, EfEventStore eventStore, IJsonSerializer serializer, Func<long?> externalCheckpointSource = null)
        {
            Ensure.NotNullOrWhiteSpace(subscriptionId, nameof(subscriptionId));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(eventStore, nameof(eventStore));

            this.eventStore = eventStore;
            this.serializer = serializer;
            this.subscriptionId = subscriptionId;

            if (externalCheckpointSource is null)
                this.hasExternalCheckpointSource = false;
            else
            {
                this.hasExternalCheckpointSource = true;
                this.externalCheckpointSource = externalCheckpointSource;
            }
        }

        public string SubscriptionStreamName => "AllEvents";

        public void SetListener(Func<long, object, Task> listener) => this.listener = listener;

        public void Start()
        {
            lock (this.lockObject)
            {
                if (this.cancellationSource == null)
                {
                    this.cancellationSource = new CancellationTokenSource();
                    this.ResolveCurrentCheckpoint();
                    this.log.Info($"Starting subscription {this.subscriptionId} from {this.SubscriptionStreamName} at " + (!this.currentCheckpoint.HasValue ? " the beginning" : $" checkpoint {this.currentCheckpoint}"));
                    this.receiveEventsTask = Task.Factory
                        .StartNew(() => this.ReceiveEvents(this.cancellationSource.Token),
                        this.cancellationSource.Token,
                        TaskCreationOptions.LongRunning,
                        TaskScheduler.Current);
                }
            }
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void ResolveCurrentCheckpoint()
        {
            if (this.currentCheckpoint != null) return;

            if (this.hasExternalCheckpointSource)
            {
                this.currentCheckpoint = this.externalCheckpointSource.Invoke();
                return;
            }

            this.currentCheckpoint = this.eventStore.GetCheckpoint(this.subscriptionId);
        }

        private void ReceiveEvents(CancellationToken cancelationToken)
        {
            while (!cancellationSource.IsCancellationRequested)
            {
                if (!this.ReceiveEvent())
                {
                    if (this.onLiveCheckpoint != this.currentCheckpoint)
                    {
                        this.onLiveCheckpoint = this.currentCheckpoint;
                        this.OnLive();
                    }
                    Thread.Sleep(100);
                }
            }
        }

        private bool ReceiveEvent()
        {
            if (!this.eventStore.ReceiveEvent(this.currentCheckpoint, out var descriptor))
                return false;

            try
            {
                this.OnEventApeared(descriptor);
            }
            catch (Exception ex)
            {
                this.OnError(ex);
            }

            return true;
        }

        private void OnEventApeared(EventDescriptor descriptor)
        {
            var newCheckpoint = descriptor.Position.Value;
            var deserialized = this.serializer.Deserialize(descriptor.Payload);
            this.listener.Invoke(newCheckpoint, deserialized);
            this.currentCheckpoint = newCheckpoint;
            if (!this.hasExternalCheckpointSource)
                this.eventStore.SaveCheckpoint(this.subscriptionId, this.currentCheckpoint);
        }

        private void OnLive()
        {
            this.log.Verbose($"The subscription {this.subscriptionId} of {this.SubscriptionStreamName} has caught-up on" + (this.currentCheckpoint.HasValue ? $" checkpoint {this.currentCheckpoint}!" : " the very beginning!"));
        }

        private void OnError(Exception ex)
        {
            // The connection issues should handle the event store. Period.
            throw ex;
        }
    }
}
