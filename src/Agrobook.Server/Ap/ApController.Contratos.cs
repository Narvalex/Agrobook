﻿using Agrobook.Domain.Ap.Messages;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    public partial class ApController
    {
        [HttpPost]
        [Route("registrar-contrato")]
        public async Task<IHttpActionResult> RegistrarContrato([FromBody]RegistrarNuevoContrato cmd)
        {
            var idContrato = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idContrato);
        }

        [HttpPost]
        [Route("editar-contrato")]
        public async Task<IHttpActionResult> EditarContrato([FromBody]EditarContrato cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-contrato/{id}")]
        public async Task<IHttpActionResult> EliminarContrato([FromUri]string id)
        {
            await this.service.HandleAsync(new EliminarContrato(null, id).ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-contrato/{id}")]
        public async Task<IHttpActionResult> RestaurarContrato([FromUri]string id)
        {
            await this.service.HandleAsync(new RestaurarContrato(null, id).ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("registrar-adenda")]
        public async Task<IHttpActionResult> RegistrarAdenda([FromBody]RegistrarNuevaAdenda cmd)
        {
            var idAdenda = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idAdenda);
        }

        [HttpPost]
        [Route("editar-adenda")]
        public async Task<IHttpActionResult> EditarAdenda([FromBody]EditarAdenda cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-adenda")]
        public async Task<IHttpActionResult> EliminarAdenda([FromUri]string idContrato, [FromUri]string idAdenda)
        {
            await this.service.HandleAsync(new EliminarAdenda(null, idContrato, idAdenda).ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-adenda")]
        public async Task<IHttpActionResult> RestaurarAdenda([FromUri]string idContrato, [FromUri]string idAdenda)
        {
            await this.service.HandleAsync(new RestaurarAdenda(null, idContrato, idAdenda).ConFirma(this.ActionContext));
            return this.Ok();
        }
    }
}
