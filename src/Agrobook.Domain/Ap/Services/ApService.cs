using Eventing.Core.Domain;
using Eventing.Core.Persistence;

namespace Agrobook.Domain.Ap.Services
{
    public class ApService : EventSourcedService
    {
        public ApService(IEventSourcedRepository repository) : base(repository)
        { }
    }
}
