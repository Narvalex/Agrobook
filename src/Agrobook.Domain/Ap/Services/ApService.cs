using Agrobook.Common;
using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using Eventing.Log;

namespace Agrobook.Domain.Ap.Services
{
    public partial class ApService : EventSourcedHandler
    {
        private readonly ILogLite logger = LogManager.GetLoggerFor<ApService>();
        private readonly IDateTimeProvider dateTimeProvider;

        public ApService(IEventSourcedRepository repository, IDateTimeProvider dateTimeProvider)
            : base(repository)
        {
            Ensure.NotNull(dateTimeProvider, nameof(dateTimeProvider));

            this.dateTimeProvider = dateTimeProvider;
        }
    }
}
