using Agrobook.Domain.Ap.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    [RoutePrefix("ap/query")]
    public class ApQueryController : ApiController
    {
        private readonly ApQueryService service = ServiceLocator.ResolveSingleton<ApQueryService>();

        [HttpGet]
        [Route("clientes")]
        public async Task<IHttpActionResult> GetClientes(string filtro)
            => this.Ok(await this.service.ObtenerClientes(filtro));

        [HttpGet]
        [Route("org/{idOrg}")]
        public async Task<IHttpActionResult> ObtenerOrg(string idOrg)
            => this.Ok(await this.service.ObtenerOrg(idOrg));
    }
}
