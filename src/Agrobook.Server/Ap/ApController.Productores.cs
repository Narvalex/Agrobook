using Agrobook.Domain.Ap.Messages;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    public partial class ApController : ApiController
    {
        [HttpPost]
        [Route("registrar-parcela")]
        public async Task<IHttpActionResult> RegistrarParcela([FromBody]RegistrarParcela cmd)
        {
            var idParcela = await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok(idParcela);
        }

        [HttpPost]
        [Route("editar-parcela")]
        public async Task<IHttpActionResult> EditarParcela([FromBody]EditarParcela cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-parcela")]
        public async Task<IHttpActionResult> EliminarParcela([FromBody]EliminarParcela cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-parcela")]
        public async Task<IHttpActionResult> RestaurarParcela([FromBody]RestaurarParcela cmd)
        {
            await this.service.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }
    }
}
