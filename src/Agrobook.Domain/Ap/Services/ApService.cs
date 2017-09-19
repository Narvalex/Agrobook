using Agrobook.Common;
using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;

namespace Agrobook.Domain.Ap.Services
{
    // Base
    public partial class ApService : EventSourcedService
    {
        private readonly IDateTimeProvider dateTimeProvider;

        public ApService(IEventSourcedRepository repository, IDateTimeProvider dateTimeProvider) : base(repository)
        {
            Ensure.NotNull(dateTimeProvider, nameof(dateTimeProvider));

            this.dateTimeProvider = dateTimeProvider;
        }
    }
}
