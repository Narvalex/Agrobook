using Agrobook.Client.Ap;
using Agrobook.Domain.Ap.Messages;
using Eventing.Client.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap")]
    public class ApController : ApiControllerBase
    {
        private readonly ApClient client;

        public ApController()
        {
            this.client = ServiceLocator.ResolveNewOf<ApClient>().WithTokenProvider(this.TokenProvider);
        }

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
    }
}