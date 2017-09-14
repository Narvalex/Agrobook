using Agrobook.Client.Ap;
using Eventing.Client.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap/query")]
    public class ApQueryController : ApiControllerBase
    {
        private readonly ApQueryClient client;

        public ApQueryController()
        {
            this.client = ServiceLocator.ResolveNewOf<ApQueryClient>().WithTokenProvider(this.TokenProvider);
        }

        [HttpGet]
        [Route("clientes")]
        public async Task<IHttpActionResult> ObtenerClientes([FromUri]string filtro)
        {
            var list = await this.client.ObtenerClientes(filtro);
            return this.Ok(list);
        }

        [HttpGet]
        [Route("org/{idOrg}")]
        public async Task<IHttpActionResult> ObtenerOrg([FromUri]string idOrg)
        {
            var org = await this.client.ObtenerOrg(idOrg);
            return this.Ok(org);
        }
    }
}