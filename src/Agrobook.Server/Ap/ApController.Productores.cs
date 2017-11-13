using Agrobook.Domain.Ap.Commands;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server.Ap
{
    public partial class ApController : ApiController
    {
        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("registrar-parcela")]
        public async Task<IHttpActionResult> RegistrarParcela([FromBody]RegistrarParcela cmd)
        {
            var idParcela = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idParcela);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("editar-parcela")]
        public async Task<IHttpActionResult> EditarParcela([FromBody]EditarParcela cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("eliminar-parcela")]
        public async Task<IHttpActionResult> EliminarParcela([FromBody]EliminarParcela cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("restaurar-parcela")]
        public async Task<IHttpActionResult> RestaurarParcela([FromBody]RestaurarParcela cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }
    }
}
