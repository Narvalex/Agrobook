using Agrobook.Domain.Ap.Messages;
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
        [Route("nuevo-servicio")]
        public async Task<IHttpActionResult> NuevoServicio([FromBody]RegistrarNuevoServicio cmd)
        {
            var idServicio = await this.numeradorDeServicios.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idServicio);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("editar-datos-basicos-del-servicio")]
        public async Task<IHttpActionResult> EditarDatosBasicosDelServicio([FromBody]EditarDatosBasicosDelSevicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("eliminar-servicio")]
        public async Task<IHttpActionResult> EliminarServicio([FromBody]EliminarServicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("restaurar-servicio")]
        public async Task<IHttpActionResult> RestaurarServicio([FromBody]RestaurarServicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("especificar-parcela-del-servicio")]
        public async Task<IHttpActionResult> EspecificarParcelaDelServicio([FromBody]EspecificarParcelaDelServicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("cambiar-parcela-del-servicio")]
        public async Task<IHttpActionResult> CambiarParcelaDelServicio([FromBody]CambiarParcelaDelServicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }
    }
}
