using Eventing.Core.Messaging;
using Eventing.Core.Serialization;
using Eventing.Log;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using System;
using System.Text;
using System.Threading;

namespace Eventing.GetEventStore.Messaging
{
    /// <summary>
    /// A subscription that receives events from the EventStore and passes to a listener or consumer.
    /// </summary>
    /// <remarks>
    /// We cannot subscribe to all because it will be forever receiving events from checkpoints being written.
    /// </remarks>
    public class EsSubscription : IEventSubscription, IDisposable
    {
        private ILogLite log = LogManager.GetLoggerFor<EsSubscription>();

        private readonly IEventStoreConnection resilientConnection;
        private readonly IJsonSerializer serializer;

        private readonly string streamName;
        private readonly string subscriptionId;
        private readonly string subscriptionCheckpointStream;
        private bool subscriptionCheckpointMetadataIsSet = false;

        private EventStoreCatchUpSubscription subscription;

        private long? currentCheckpoint = null;

        private readonly object lockObject = new object();
        private CancellationTokenSource cancellationSource = null;

        private bool hasExternalCheckpointSource;
        private Func<long?> externalCheckpointSource;

        private Action<long, object> listener;

        private ProjectionDefinition projectionDefinition = null;

        /// <summary>
        /// Inializes a new instance of the <see cref="EsSubscription"/> class.
        /// </summary>
        /// <param name="streamName">The specific event stream from which events will be received.</param>
        /// <param name="subscriptionId">The identifier of the subscription, useful for logging and to store checkpoints if applicable.</para
        /// <param name="resilientConnection">The resilient connection to the EventStore database.</param>
        /// <param name="serializer">The serializer to use for the event body.</param>
        /// <param name="externalCheckpointSource">The source of the checkpoints. If the value is null, then the EventStore will be the source of the checkpoint.</param>
        public EsSubscription(string streamName, string subscriptionId, IEventStoreConnection resilientConnection, IJsonSerializer serializer, Func<long?> externalCheckpointSource = null)
        {
            Ensure.NotNull(resilientConnection, nameof(resilientConnection));
            Ensure.NotNullOrWhiteSpace(subscriptionId, nameof(subscriptionId));
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNullOrWhiteSpace(streamName, nameof(streamName));

            this.resilientConnection = resilientConnection;
            this.streamName = streamName;
            this.subscriptionId = subscriptionId;
            this.serializer = serializer;
            if (externalCheckpointSource is null)
            {
                this.hasExternalCheckpointSource = false;
                this.subscriptionCheckpointStream = $"eventing.checkpoints-{this.subscriptionId}";
            }
            else
            {
                this.hasExternalCheckpointSource = true;
                this.externalCheckpointSource = externalCheckpointSource;
            }
        }

        /// <summary>
        /// Inializes a new instance of the <see cref="EsSubscription"/> class.
        /// </summary>
        /// <remarks>
        /// There is a problem to subscribe to all events. When the subscription persists checkpoints in the EventStore, the subscription will receive those events that 
        /// will end up filling all available storage.
        /// </remarks>
        /// <param name="streamName">The specific event stream from which events will be received.</param>
        /// <param name="subscriptionId">The identifier of the subscription, useful for logging and to store checkpoints if applicable.</para
        /// <param name="resilientConnection">The resilient connection to the EventStore database.</param>
        /// <param name="serializer">The serializer to use for the event body.</param>
        /// <param name="externalCheckpointSource">The source of the checkpoints. If the value is null, then the EventStore will be the source of the checkpoint.</param>
        public EsSubscription(ProjectionDefinition projectionDefinition, string subscriptionId, IEventStoreConnection resilientConnection, IJsonSerializer serializer, Func<long?> externalCheckpointSource = null)
            : this(projectionDefinition.EmittedStream, subscriptionId, resilientConnection, serializer, externalCheckpointSource)
        {
            Ensure.NotNull(projectionDefinition, nameof(projectionDefinition));

            this.projectionDefinition = projectionDefinition;
        }

        public void Start()
        {
            lock (this.lockObject)
            {
                if (this.cancellationSource == null)
                {
                    this.cancellationSource = new CancellationTokenSource();
                    if (this.projectionDefinition != null) this.projectionDefinition.EnsureThatIsUpToDateAndRunning().Wait();
                    this.log.Info($"Starting subscription {this.subscriptionId} from {this.streamName} at" + (!this.currentCheckpoint.HasValue ? " the beginning" : $" checkpoint {this.currentCheckpoint}"));
                    this.DoStart();
                }
            }
        }

        public void Stop()
        {
            lock (this.lockObject)
            {
                using (this.cancellationSource)
                {
                    if (this.cancellationSource != null)
                    {
                        this.subscription?.Stop();
                        this.cancellationSource.Cancel();
                        this.cancellationSource = null;
                    }
                }
            }
        }

        public void SetListener(Action<long, object> listener)
        {
            this.listener = listener;
        }

        private void DoStart()
        {
            this.ResolveCurrentCheckpoint();
            this.subscription = this.resilientConnection.SubscribeToStreamFrom(this.streamName, this.currentCheckpoint, CatchUpSubscriptionSettings.Default,
                onEventApeared, onLive, onError);

            void onEventApeared(EventStoreCatchUpSubscription sub, ResolvedEvent eventAppeared)
            {
                var newCheckpoint = eventAppeared.OriginalEventNumber;
                var deserialized = this.Deserialize(eventAppeared);
                this.listener.Invoke(newCheckpoint, deserialized);
                this.currentCheckpoint = newCheckpoint;
                if (!this.hasExternalCheckpointSource)
                    this.PersistCurrentCheckpoint();
            }

            void onLive(EventStoreCatchUpSubscription sub)
                => this.log.Verbose($"The subscription {this.subscriptionId} of {this.streamName} has caught-up on" + (this.currentCheckpoint.HasValue ? $" checkpoint {this.currentCheckpoint}!" : " the very beginning!"));

            void onError(EventStoreCatchUpSubscription sub, SubscriptionDropReason reason, Exception ex)
            {
                // See here: https://groups.google.com/forum/#!searchin/event-store/subscription/event-store/AdKzv8TxabM/6RzudeuAAgAJ

                if (reason == SubscriptionDropReason.UserInitiated) return;

                if (reason == SubscriptionDropReason.ConnectionClosed || reason == SubscriptionDropReason.CatchUpError)
                {
                    var seconds = 3;
                    var chkp = this.currentCheckpoint.HasValue ? this.currentCheckpoint : -1;
                    var message = $"The subscription {this.subscriptionId} of {this.streamName} stopped because of {reason} on checkpoint {chkp}. Restarting in {seconds} seconds.";
                    if (reason == SubscriptionDropReason.ConnectionClosed)
                        this.log.Info(message);
                    else if (reason == SubscriptionDropReason.CatchUpError && ex is NotAuthenticatedException)
                    {
                        seconds = 2;
                        message = $"The connection was not authenticated yet. If this persist you should check the credentianls. The subscription {this.subscriptionId} of {this.streamName} stopped on checkpoint {chkp}. Retrying in {seconds} seconds.";
                        this.log.Warning(message);
                    }
                    else
                        this.log.Error(ex, message);

                    this.subscription.Stop();
                    Thread.Sleep(TimeSpan.FromSeconds(seconds));
                    this.log.Info($"Restarting subscription {this.subscriptionId} of {this.streamName} on checkpoint {chkp}");
                    this.DoStart();
                    return;
                }

                // This should be handled better
                this.Stop();
                throw ex;
            }
        }

        private void ResolveCurrentCheckpoint()
        {
            // The checkpoint is live in this component.
            if (this.currentCheckpoint != null) return;

            if (this.hasExternalCheckpointSource)
            {
                // The checkpoint is being obtained from an external source, like a SQL database
                this.currentCheckpoint = this.externalCheckpointSource.Invoke();
                return;
            }

            var readResult = this.resilientConnection.ReadEventAsync(this.subscriptionCheckpointStream, StreamPosition.End, false).Result;
            if (readResult.Status != EventReadStatus.Success)
                // There is not a checkpoint yet. This looks like a fresh start...
                return;

            var deserialized = this.Deserialize(readResult.Event.Value);
            // This is a checkpoint obtained from the EventStore.
            this.currentCheckpoint = ((SubscriptionCheckpoint)deserialized).EventNumber;
        }

        private void PersistCurrentCheckpoint()
        {
            var e = new SubscriptionCheckpoint(this.currentCheckpoint.Value);
            var serialized = this.serializer.Serialize(e);
            var bytes = Encoding.UTF8.GetBytes(serialized);
            var eventData = new EventData(Guid.NewGuid(), SubscriptionCheckpoint.eventTypeName, true, bytes, null);
            this.resilientConnection.AppendToStreamAsync(this.subscriptionCheckpointStream, ExpectedVersion.Any, eventData).Wait();

            if (this.subscriptionCheckpointMetadataIsSet) return;

            // Setting the metadata
            var result = this.resilientConnection.GetStreamMetadataAsync(this.subscriptionCheckpointStream).Result;
            if (!result.StreamMetadata.MaxCount.HasValue || result.StreamMetadata.MaxCount != 1)
                this.resilientConnection.SetStreamMetadataAsync(this.subscriptionCheckpointStream, ExpectedVersion.Any,
                    StreamMetadata.Build().SetMaxCount(1).Build())
                    .Wait();
            this.subscriptionCheckpointMetadataIsSet = true;
        }

        private object Deserialize(ResolvedEvent e)
        {
            var serialized = Encoding.UTF8.GetString(e.Event.Data);
            var deserialized = this.serializer.Deserialize(serialized);
            return deserialized;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) => this.Stop();

        ~EsSubscription() => Dispose(false);
    }
}
