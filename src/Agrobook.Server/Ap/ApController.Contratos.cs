using Agrobook.Domain.Ap.Commands;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server.Ap
{
    public partial class ApController
    {
        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("registrar-contrato")]
        public async Task<IHttpActionResult> RegistrarContrato([FromBody]RegistrarNuevoContrato cmd)
        {
            var idContrato = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idContrato);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("editar-contrato")]
        public async Task<IHttpActionResult> EditarContrato([FromBody]EditarContrato cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("eliminar-contrato/{id}")]
        public async Task<IHttpActionResult> EliminarContrato([FromUri]string id)
        {
            await this.service.HandleAsync(new EliminarContrato(null, id).ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("restaurar-contrato/{id}")]
        public async Task<IHttpActionResult> RestaurarContrato([FromUri]string id)
        {
            await this.service.HandleAsync(new RestaurarContrato(null, id).ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("registrar-adenda")]
        public async Task<IHttpActionResult> RegistrarAdenda([FromBody]RegistrarNuevaAdenda cmd)
        {
            var idAdenda = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idAdenda);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("editar-adenda")]
        public async Task<IHttpActionResult> EditarAdenda([FromBody]EditarAdenda cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("eliminar-adenda")]
        public async Task<IHttpActionResult> EliminarAdenda([FromUri]string idContrato, [FromUri]string idAdenda)
        {
            await this.service.HandleAsync(new EliminarAdenda(null, idContrato, idAdenda).ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("restaurar-adenda")]
        public async Task<IHttpActionResult> RestaurarAdenda([FromUri]string idContrato, [FromUri]string idAdenda)
        {
            await this.service.HandleAsync(new RestaurarAdenda(null, idContrato, idAdenda).ConFirma(this.ActionContext));
            return this.Ok();
        }
    }
}

