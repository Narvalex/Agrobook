using Agrobook.Core;
using EventStore.ClientAPI;
using System;
using System.Text;

namespace Agrobook.Infrastructure.Subscription
{
    public class EventStreamSubscription : IEventStreamSubscription
    {
        private readonly IEventStoreConnection resilientConnection;
        private readonly IJsonSerializer serializer;
        private readonly string streamName;
        private readonly Action<long, object> handler;
        private readonly TimeSpan stopTimeout;

        private Lazy<long?> lastCheckpoint;
        private EventStoreCatchUpSubscription subscription;

        private bool shouldStopNow = false;

        private readonly object lockObject = new object();

        public EventStreamSubscription(IEventStoreConnection resilientConnection, IJsonSerializer serializer, string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler, TimeSpan stopTimeout)
        {
            Ensure.NotNull(resilientConnection, nameof(resilientConnection));
            Ensure.NotNullOrWhiteSpace(streamName, nameof(streamName));
            Ensure.NotNull(handler, nameof(handler));
            Ensure.NotNull(serializer, nameof(serializer));

            this.resilientConnection = resilientConnection;
            this.streamName = streamName;
            this.handler = handler;
            this.lastCheckpoint = lastCheckpoint;
            this.stopTimeout = stopTimeout;
            this.serializer = serializer;
        }

        public void Start()
        {
            this.shouldStopNow = false;
            this.DoStart();
        }

        public void Stop()
        {
            lock (this.lockObject)
            {
                this.shouldStopNow = true;
                if (this.subscription != null)
                    this.DoStop();
            }
        }

        private void DoStart()
        {
            lock (this.lockObject)
            {
                if (this.subscription != null)
                    this.DoStop();


                this.subscription = this.resilientConnection.SubscribeToStreamFrom(this.streamName, this.lastCheckpoint.Value, CatchUpSubscriptionSettings.Default,
                       (sub, eventAppeared) =>
                       {
                           if (!this.shouldStopNow)
                           {
                               var serialized = Encoding.UTF8.GetString(eventAppeared.Event.Data);
                               var deserialized = this.serializer.Deserialize(serialized);
                               this.handler.Invoke(eventAppeared.OriginalEventNumber, deserialized);
                           }
                       },
                       sub => { },
                       (sub, reason, ex) =>
                       {
                           if (reason == SubscriptionDropReason.CatchUpError)
                           {
                               this.DoStart();
                               return;
                           }
                           else if (reason == SubscriptionDropReason.UserInitiated)
                               return;

                           throw ex;
                       });
            }
        }

        private void DoStop()
        {
            this.subscription.Stop(this.stopTimeout);
            this.subscription = null;
        }
    }
}
