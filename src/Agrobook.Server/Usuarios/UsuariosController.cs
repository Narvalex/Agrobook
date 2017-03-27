using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Usuarios
{
    [Autorizar(Claims.Roles.Admin)]
    [RoutePrefix("usuarios")]
    public class UsuariosController : ApiController
    {
        private readonly UsuariosYGruposService service = ServiceLocator.ResolveSingleton<UsuariosYGruposService>();

        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuarioAsync([FromBody]CrearNuevoUsuario command)
        {
            await this.service.HandleAsync(command);
            return this.Ok();
        }
    }
}
