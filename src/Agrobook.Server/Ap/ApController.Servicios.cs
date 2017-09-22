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
            var idServicio = await this.service.HandleAsync(cmd);
            return this.Ok(idServicio);
        }
    }
}
