using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios")]
    public class UsuariosController : ApiController
    {
        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuarioAsync()
        {
            return await Task.FromResult(this.Ok());
        }
    }
}
