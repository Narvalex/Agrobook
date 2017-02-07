namespace Agrobook.Core
{
    public abstract class CommandHandler
    {
        protected readonly IEventSourcedRepository repository;

        public CommandHandler(IEventSourcedRepository repository)
        {
            Ensure.NotNull(repository, nameof(repository));

            this.repository = repository;
        }
    }
}
