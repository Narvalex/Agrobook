using Agrobook.Domain.Ap.Commands;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    public partial class ApController
    {
        [HttpPost]
        [Route("registrar-parcela")]
        public async Task<IHttpActionResult> RegistrarParcela([FromBody]RegistrarParcela cmd)
        {
            var idParcela = await this.client.Send(cmd);
            return this.Ok(idParcela);
        }

        [HttpPost]
        [Route("editar-parcela")]
        public async Task<IHttpActionResult> EditarParcela([FromBody]EditarParcela cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-parcela")]
        public async Task<IHttpActionResult> EliminarParcela([FromBody]EliminarParcela cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-parcela")]
        public async Task<IHttpActionResult> RestaurarParcela([FromBody]RestaurarParcela cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }
    }
}