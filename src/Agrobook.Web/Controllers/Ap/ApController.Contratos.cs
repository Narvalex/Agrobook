using Agrobook.Domain.Ap.Messages;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    public partial class ApController
    {
        [HttpPost]
        [Route("registrar-contrato")]
        public async Task<IHttpActionResult> RegistrarContrato([FromBody]RegistrarNuevoContrato cmd)
        {
            var idContrato = await this.client.Send(cmd);
            return this.Ok(idContrato);
        }

        [HttpPost]
        [Route("editar-contrato")]
        public async Task<IHttpActionResult> EditarContrato([FromBody]EditarContrato cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-contrato/{id}")]
        public async Task<IHttpActionResult> EliminarContrato([FromUri]string id)
        {
            await this.client.EliminarContrato(id);
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-contrato/{id}")]
        public async Task<IHttpActionResult> RestaurarContrato([FromUri]string id)
        {
            await this.client.RestaurarContrato(id);
            return this.Ok();
        }

        [HttpPost]
        [Route("registrar-adenda")]
        public async Task<IHttpActionResult> RegistrarAdenda([FromBody]RegistrarNuevaAdenda cmd)
        {
            var idContrato = await this.client.Send(cmd);
            return this.Ok(idContrato);
        }

        [HttpPost]
        [Route("editar-adenda")]
        public async Task<IHttpActionResult> EditarContrato([FromBody]EditarAdenda cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-adenda")]
        public async Task<IHttpActionResult> EliminarAdenda([FromUri]string idContrato, [FromUri]string idAdenda)
        {
            await this.client.EliminarAdenda(idContrato, idAdenda);
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-adenda")]
        public async Task<IHttpActionResult> RestaurarAdenda([FromUri]string idContrato, [FromUri]string idAdenda)
        {
            await this.client.RestaurarAdenda(idContrato, idAdenda);
            return this.Ok();
        }
    }
}