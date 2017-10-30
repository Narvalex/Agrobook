using Eventing.Core.Persistence;

namespace Eventing.Core.Domain
{
    public abstract class EventSourcedService
    {
        protected readonly IEventSourcedRepository repository;

        public EventSourcedService(IEventSourcedRepository repository)
        {
            Ensure.NotNull(repository, nameof(repository));

            this.repository = repository;
        }
    }
}
