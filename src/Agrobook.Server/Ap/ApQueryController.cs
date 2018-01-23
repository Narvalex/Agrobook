using Agrobook.Domain.Ap.Services;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server.Ap
{
    [RoutePrefix("ap/query")]
    public class ApQueryController : ApiController
    {
        private readonly ApQueryService service = ServiceLocator.ResolveSingleton<ApQueryService>();

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("clientes")]
        public async Task<IHttpActionResult> GetClientes(string filtro)
            => this.Ok(await this.service.ObtenerClientes(filtro));

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("org/{idOrg}")]
        public async Task<IHttpActionResult> ObtenerOrg(string idOrg)
            => this.Ok(await this.service.ObtenerOrg(idOrg));

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("contratos/{idOrg}")]
        public async Task<IHttpActionResult> ObtenerContratos(string idOrg)
            => this.Ok(await this.service.ObtenerContratos(idOrg));

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor)]
        [HttpGet]
        [Route("parcelas/{idProd}")]
        public async Task<IHttpActionResult> GetParcelas([FromUri] string idProd)
            => this.Ok(await this.service.ObtenerParcelas(idProd));

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor)]
        [HttpGet]
        [Route("parcela/{idParcela}")]
        public async Task<IHttpActionResult> GetParcela([FromUri] string idParcela)
            => this.Ok(await this.service.ObtenerParcela(idParcela));

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("orgs-con-contratos-del-productor/{idProd}")]
        public async Task<IHttpActionResult> GetOrgsConContratosDelProductor([FromUri]string idProd)
            => this.Ok(await this.service.ObtenerOrgsConContratosDelProductor(idProd));

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor)]
        [HttpGet]
        [Route("prod/{idProd}")]
        public async Task<IHttpActionResult> GetProd([FromUri]string idProd)
        {
            var prod = await this.service.GetProd(idProd);
            return this.Ok(prod);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("servicios-por-org/{idOrg}")]
        public async Task<IHttpActionResult> GetServiciosPorOrg([FromUri]string idOrg)
        {
            var servicios = await this.service.GetServiciosPorOrg(idOrg);
            return this.Ok(servicios);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("servicios-por-org-agrupados-por-contrato/{idOrg}")]
        public async Task<IHttpActionResult> GetServiciosPorOrgAgrupadosPorContrato([FromUri]string idOrg)
        {
            var servicios = await this.service.GetServiciosPorOrgAgrupadosPorContrato(idOrg);
            return this.Ok(servicios);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor)]
        [HttpGet]
        [Route("servicios-por-prod/{idProd}")]
        public async Task<IHttpActionResult> GetServiciosPorProd([FromUri]string idProd)
        {
            var servicios = await this.service.GetServiciosPorProd(idProd);
            return this.Ok(servicios);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor)]
        [HttpGet]
        [Route("servicio/{idServicio}")]
        public async Task<IHttpActionResult> GetServicio([FromUri]string idServicio)
        {
            var servicio = await this.service.GetServicio(idServicio);
            return this.Ok(servicio);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("ultimos-servicios")]
        public async Task<IHttpActionResult> GetUltimosServicios([FromUri]int cantidad)
        {
            var list = await this.service.GetUltimosServicios(cantidad);
            return this.Ok(list);
        }
    }
}
