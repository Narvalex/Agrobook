namespace Agrobook.Core
{
    public interface IEventSourcedRepository
    {
        // returns null when not found
        T Get<T>(string streamName) where T : class, IEventSourced, new();

        // throws an exception when concurrency check fails
        void Save<T>(T updatedState) where T : class, IEventSourced, new();
    }
}
