﻿using Agrobook.Domain.Ap.Services;
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

        [HttpGet]
        [Route("orgs-con-contratos-del-productor/{idProd}")]
        public async Task<IHttpActionResult> GetOrgsConContratosDelProductor([FromUri]string idProd)
            => this.Ok(await this.service.ObtenerOrgsConContratosDelProductor(idProd));

        [HttpGet]
        [Route("prod/{idProd}")]
        public async Task<IHttpActionResult> GetProd([FromUri]string idProd)
        {
            var prod = await this.service.GetProd(idProd);
            return this.Ok(prod);
        }

        [HttpGet]
        [Route("servicios-por-org/{idOrg}")]
        public async Task<IHttpActionResult> GetServiciosPorOrg([FromUri]string idOrg)
        {
            var servicios = await this.service.GetServiciosPorOrg(idOrg);
            return this.Ok(servicios);
        }

        [HttpGet]
        [Route("servicios-por-prod/{idProd}")]
        public async Task<IHttpActionResult> GetServiciosPorProd([FromUri]string idProd)
        {
            var servicios = await this.service.GetServiciosPorProd(idProd);
            return this.Ok(servicios);
        }

        [HttpGet]
        [Route("servicio/{idServicio}")]
        public async Task<IHttpActionResult> GetServicio([FromUri]string idServicio)
        {
            var servicio = await this.service.GetServicio(idServicio);
            return this.Ok(servicio);
        }

        [HttpGet]
        [Route("ultimos-servicios")]
        public async Task<IHttpActionResult> GetUltimosServicios([FromUri]int cantidad)
        {
            var list = await this.service.GetUltimosServicios(cantidad);
            return this.Ok(list);
        }
    }
}
