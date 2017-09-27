using Agrobook.Domain.Ap.Messages;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    public partial class ApController
    {
        [HttpPost]
        [Route("nuevo-servicio")]
        public async Task<IHttpActionResult> NuevoServicio([FromBody]RegistrarNuevoServicio cmd)
        {
            var idServicio = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idServicio);
        }

        [HttpPost]
        [Route("editar-datos-basicos-del-servicio")]
        public async Task<IHttpActionResult> EditarDatosBasicosDelServicio([FromBody]EditarDatosBasicosDelSevicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-servicio")]
        public async Task<IHttpActionResult> EliminarServicio([FromBody]EliminarServicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-servicio")]
        public async Task<IHttpActionResult> RestaurarServicio([FromBody]RestaurarServicio cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }
    }
}
