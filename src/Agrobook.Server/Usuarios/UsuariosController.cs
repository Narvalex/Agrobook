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
        private readonly UsuariosService service = ServiceLocator.ResolveSingleton<UsuariosService>();

        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuarioAsync([FromBody]CrearNuevoUsuario command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("actualizar-perfil")]
        public async Task<IHttpActionResult> ActualizarPerfil([FromBody]ActualizarPerfil command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("resetear-password")]
        public async Task<IHttpActionResult> ResetearPassword([FromBody]ResetearPassword command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("crear-nueva-organizacion")]
        public async Task<IHttpActionResult> CrearNuevaOrganizacion([FromBody]CrearNuevaOrganizacion command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }
    }
}
