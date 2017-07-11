using Agrobook.Domain.Usuarios;
using Agrobook.Server.Filters;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

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
            var puedeProceder = false;
            var claims = this.service.GetClaims(this.ActionContext.GetToken());

            if (claims.Any(x => x == Roles.Admin))
                puedeProceder = true;

            else if (claims.Any(x => x == Roles.Gerente))
                puedeProceder = !command.Claims.Any(x => x == Roles.Admin || x == Roles.Gerente);

            else if (claims.Any(x => x == Roles.Tecnico))
                puedeProceder = !command.Claims.Any(x => x == Roles.Admin || x == Roles.Tecnico || x == Roles.Tecnico);

            if (puedeProceder)
            {
                await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
                return this.Ok();
            }

            return this.Unauthorized();
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
            var grupo = await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok(grupo);
        }

        [HttpPost]
        [Route("agregar-usuario-a-la-organizacion")]
        public async Task<IHttpActionResult> AgregarUsuarioALaOrganizacion([FromBody]AgregarUsuarioALaOrganizacion command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("agregar-usuario-a-grupo")]
        public async Task<IHttpActionResult> AgregarUsuarioAGrupo([FromBody]AgregarUsuarioAUnGrupo command)
        {
            await this.service.HandleAsync(command.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("otorgar-permiso")]
        public async Task<IHttpActionResult> OtorgarPermiso([FromBody]OtorgarPermiso cmd)
        {
            await this.service.HandleAsync(cmd.ConMetadatos(this.ActionContext));
            return this.Ok();
        }

        [HttpPost]
        [Route("retirar-permiso")]
        public async Task<IHttpActionResult> RetirarPermiso([FromBody]RetirarPermiso cmd)
        {
            await this.service.HandleAsync(cmd.ConMetadatos(this.ActionContext));
            return this.Ok();
        }
    }
}
