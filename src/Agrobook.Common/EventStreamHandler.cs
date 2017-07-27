using System.Threading.Tasks;

namespace Agrobook.Common
{
    public abstract class EventStreamHandler
    {
        protected void Dispatch(long eventNumber, object @event)
        {
            ((dynamic)this).Handle(eventNumber, (dynamic)@event).Wait();
        }

        protected virtual async Task Handle(long eventNumber, object @event)
        {
            await Task.CompletedTask;
        }
    }

    public interface IEventHandler<T>
    {
        Task Handle(long eventNumber, T e);
    }
}
