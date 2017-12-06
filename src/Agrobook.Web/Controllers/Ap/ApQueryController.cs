using Agrobook.Client.Ap;
using Agrobook.Domain.Common.Services;
using Eventing.Client.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap/query")]
    public partial class ApQueryController : ApiControllerBase
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

        [HttpGet]
        [Route("contratos/{idOrg}")]
        public async Task<IHttpActionResult> ObtenerContratos([FromUri]string idOrg)
        {
            var contratos = await this.client.ObtenerContratos(idOrg);
            return this.Ok(contratos);
        }

        [HttpGet]
        [Route("parcelas/{idProd}")]
        public async Task<IHttpActionResult> GetParcelas([FromUri]string idProd)
        {
            var parcelas = await this.client.ObtenerParcelas(idProd);
            return this.Ok(parcelas);
        }

        [HttpGet]
        [Route("parcela")]
        public async Task<IHttpActionResult> GetParcela([FromUri]string idParcela)
        {
            var parcela = await this.client.ObtenerParcela(idParcela);
            return this.Ok(parcela);
        }

        [HttpGet]
        [Route("orgs-con-contratos-del-productor/{idProd}")]
        public async Task<IHttpActionResult> GetOrgConContratosDelProductor([FromUri]string idProd)
        {
            var orgs = await this.client.ObtenerOrgsConContratosDelProductor(idProd);
            return this.Ok(orgs);
        }

        [HttpGet]
        [Route("prod/{idProd}")]
        public async Task<IHttpActionResult> GetProd([FromUri]string idProd)
        {
            var prod = await this.client.GetProd(idProd);
            return this.Ok(prod);
        }

        [HttpGet]
        [Route("servicios-por-org/{idOrg}")]
        public async Task<IHttpActionResult> GetServiciosPorOrg([FromUri]string idOrg)
        {
            var servicios = await this.client.GetServiciosPorOrg(idOrg);
            return this.Ok(servicios);
        }

        [HttpGet]
        [Route("servicios-por-prod/{idProd}")]
        public async Task<IHttpActionResult> GetServiciosPorProd([FromUri]string idProd)
        {
            var servicios = await this.client.GetServiciosPorProd(idProd);
            return this.Ok(servicios);
        }

        [HttpGet]
        [Route("servicio/{idServicio}")]
        public async Task<IHttpActionResult> GetServicio([FromUri]string idServicio)
        {
            var servicio = await this.client.GetServicio(idServicio);
            return this.Ok(servicio);
        }

        [HttpGet]
        [Route("ultimos-servicios")]
        public async Task<IHttpActionResult> GetUltimosServicios([FromUri]int cantidad)
        {
            var list = await this.client.GetUltimosServicios(cantidad);
            return this.Ok(list);
        }

        [HttpGet]
        [Route("departamentos")]
        public IHttpActionResult GetDepartamentos()
        {
            var list = DepartamentosDelParaguayProvider.GetDepartamentos();
            return this.Ok(list);
        }
    }
}