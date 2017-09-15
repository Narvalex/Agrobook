using Eventing.Core.Domain;
using Eventing.Core.Persistence;

namespace Agrobook.Domain.Ap.Services
{
    // Base
    public partial class ApService : EventSourcedService
    {
        public ApService(IEventSourcedRepository repository) : base(repository)
        { }
    }
}
