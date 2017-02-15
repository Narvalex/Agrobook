namespace Agrobook.Core
{
    public abstract class EventSourcedService
    {
        protected readonly IEventSourcedRepository repository;
        protected readonly IDateTimeProvider dateTime;

        public EventSourcedService(IEventSourcedRepository repository, IDateTimeProvider dateTime)
        {
            Ensure.NotNull(repository, nameof(repository));
            Ensure.NotNull(dateTime, nameof(dateTime));

            this.repository = repository;
            this.dateTime = dateTime;
        }
    }
}
