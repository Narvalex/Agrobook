using Agrobook.Domain.Usuarios;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimsDefs;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios")]
    public class UsuariosController : ApiController
    {
        private readonly UsuariosService service = ServiceLocator.ResolveSingleton<UsuariosService>();

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
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
            var result = await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok(result);
        }

        [HttpPost]
        [Route("crear-nuevo-grupo")]
        public async Task<IHttpActionResult> CrearNuevoGrupo([FromBody]CrearNuevoGrupo command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("agregar-usuario-a-la-organizacion")]
        public async Task<IHttpActionResult> AgregarUsaurioALaOrganizacion([FromBody]AgregarUsuarioALaOrganizacion command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }
    }
}
