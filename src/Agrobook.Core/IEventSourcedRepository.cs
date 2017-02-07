namespace Agrobook.Core
{
    public interface IEventSourcedRepository
    {
        T Get<T>(string streamName) where T : IEventSourced;

        void Persist<T>(T updatedState) where T : IEventSourced;
    }
}
