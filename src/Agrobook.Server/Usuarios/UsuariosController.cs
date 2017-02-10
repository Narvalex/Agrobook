using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios")]
    public class UsuariosController : ApiController
    {
        [HttpPost]
        [Route("nuevo-usuario")]
        public async Task<IHttpActionResult> NuevoUsuarioAsync()
        {
            return await Task.FromResult(this.Ok());
        }
    }
}
