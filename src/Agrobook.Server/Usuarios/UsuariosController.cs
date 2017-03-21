using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Server.Authorization;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Usuarios
{
    [Autorizar(Claims.Roles.Tecnico)]
    [RoutePrefix("usuarios")]
    public class UsuariosController : ApiController
    {
        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuarioAsync([FromBody]CrearNuevoUsuario command)
        {
            return await Task.FromResult(this.Ok());
        }
    }
}
