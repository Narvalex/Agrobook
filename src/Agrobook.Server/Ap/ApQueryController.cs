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

        [HttpGet]
        [Route("contratos/{idOrg}")]
        public async Task<IHttpActionResult> ObtenerContratos(string idOrg)
            => this.Ok(await this.service.ObtenerContratos(idOrg));

        [HttpGet]
        [Route("parcelas/{idProd}")]
        public async Task<IHttpActionResult> GetParcelas([FromUri] string idProd)
            => this.Ok(await this.service.ObtenerParcelas(idProd));

        [HttpGet]
        [Route("parcela/{idParcela}")]
        public async Task<IHttpActionResult> GetParcela([FromUri] string idParcela)
            => this.Ok(await this.service.ObtenerParcela(idParcela));
    }
}
