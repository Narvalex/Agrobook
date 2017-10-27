namespace Eventing.Core.Messaging
{
    public interface IEventSubscription
    {
        void Start();
        void Stop();
    }
}
