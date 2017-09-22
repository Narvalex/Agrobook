using Agrobook.Domain.Ap.Messages;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    public partial class ApController
    {
        [HttpPost]
        [Route("nuevo-servicio")]
        public async Task<IHttpActionResult> NuevoServicio([FromBody]RegistrarNuevoServicio cmd)
        {
            var idServicio = await this.client.Send(cmd);
            return this.Ok(idServicio);
        }
    }
}