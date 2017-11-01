using Eventing;
using Eventing.Core.Persistence;

namespace Agrobook
{
    public abstract class EventSourcedHandler
    {
        protected readonly IEventSourcedRepository repository;

        public EventSourcedHandler(IEventSourcedRepository repository)
        {
            Ensure.NotNull(repository, nameof(repository));

            this.repository = repository;
        }
    }
}
