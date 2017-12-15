using Agrobook.Client.Ap;
using Eventing.Client.Http;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap")]
    public partial class ApController : ApiControllerBase
    {
        private readonly IApClient client;

        public ApController()
        {
            this.client = ServiceLocator.ResolveNewOf<IApClient>().WithTokenProvider(this.TokenProvider);
        }
    }
}