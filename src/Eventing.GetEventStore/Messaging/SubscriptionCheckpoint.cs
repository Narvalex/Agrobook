namespace Eventing.GetEventStore.Messaging
{
    public class SubscriptionCheckpoint
    {
        public const string eventTypeName = "subscriptionCheckpoint";

        public SubscriptionCheckpoint(long eventNumber)
        {
            this.EventNumber = eventNumber;
        }

        public long EventNumber { get; }
    }
}
