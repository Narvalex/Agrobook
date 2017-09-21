using Agrobook.Client.Ap;
using Eventing.Client.Http;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap")]
    public partial class ApController : ApiControllerBase
    {
        private readonly ApClient client;

        public ApController()
        {
            this.client = ServiceLocator.ResolveNewOf<ApClient>().WithTokenProvider(this.TokenProvider);
        }
    }
}