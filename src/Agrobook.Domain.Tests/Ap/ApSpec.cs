using Agrobook.Domain.Ap.Services;
using Eventing.TestHelpers;

namespace Agrobook.Domain.Tests.Ap
{
    public class ApSpec
    {
        protected TestableEventSourcedService<ApService> sut;

        public ApSpec()
        {
            this.sut = new TestableEventSourcedService<ApService>(r => new ApService(r));
        }
    }
}
