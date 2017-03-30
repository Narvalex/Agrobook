namespace Agrobook.Core
{
    public interface IEventStreamSubscription
    {
        void Start();
        void Stop();
    }
}
