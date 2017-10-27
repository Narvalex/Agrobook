using System.Threading.Tasks;

namespace Agrobook.Common
{
    public interface IEventHandler<T>
    {
        Task HandleOnce(long eventNumber, T e);
    }

    public abstract class EventStreamDenormalizer
    {
        protected void Dispatch(long eventNumber, object @event)
        {
            ((dynamic)this).HandleOnce(eventNumber, (dynamic)@event).Wait();
        }

        protected virtual async Task HandleOnce(long eventNumber, object @event)
        {
            await Task.CompletedTask;
        }
    }
}
