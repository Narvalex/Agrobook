namespace Eventing.Core.Persistence
{
    public interface IEventSubscription
    {
        void Start();
        void Stop();
    }
}
