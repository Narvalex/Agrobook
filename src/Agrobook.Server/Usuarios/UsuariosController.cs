using Agrobook.Domain.Usuarios;
using Agrobook.Server.Filters;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios")]
    public class UsuariosController : BaseApiController
    {
        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuarioAsync([FromBody]CrearNuevoUsuario command)
        {
            var puedeProceder = false;
            var claims = this.usuariosService.GetClaims(this.ActionContext.GetToken());

            if (claims.Any(x => x == Roles.Admin))
                puedeProceder = true;

            else if (claims.Any(x => x == Roles.Gerente))
                puedeProceder = !command.Claims.Any(x => x == Roles.Admin || x == Roles.Gerente);

            else if (claims.Any(x => x == Roles.Tecnico))
                puedeProceder = !command.Claims.Any(x => x == Roles.Admin || x == Roles.Tecnico || x == Roles.Tecnico);

            if (puedeProceder)
            {
                await this.usuariosService.HandleAsync(command.ConFirma(this.ActionContext));
                return this.Ok();
            }

            return this.Unauthorized();
        }

        [Autorizar(Roles.Gerente, Roles.Productor, Roles.Tecnico)]
        [HttpPost]
        [Route("actualizar-perfil")]
        public async Task<IHttpActionResult> ActualizarPerfil([FromBody]ActualizarPerfil command)
        {
            var user = this.usuariosService.GetCurrentUser(this.ActionContext.GetToken());
            command.ElQueRealizaEstaAccion = user;

            var seAutorizo = await this.usuariosService.HandleAsync(command.ConFirma(this.ActionContext));
            if (seAutorizo) return this.Ok();
            return this.Unauthorized();
        }

        [Autorizar(Roles.Gerente, Roles.Productor, Roles.Tecnico)]
        [HttpPost]
        [Route("resetear-password")]
        public async Task<IHttpActionResult> ResetearPassword([FromBody]ResetearPassword command)
        {
            var user = this.usuariosService.GetCurrentUser(this.ActionContext.GetToken());
            command.ElQueRealizaEstaAccion = user;
            if (await this.usuariosService.HandleAsync(command.ConFirma(this.ActionContext)))
                return this.Ok();
            return this.Unauthorized();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("crear-nueva-organizacion")]
        public async Task<IHttpActionResult> CrearNuevaOrganizacion([FromBody]CrearNuevaOrganizacion command)
        {
            var result = await this.usuariosService.HandleAsync(command.ConFirma(this.ActionContext));
            return this.Ok(result);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("eliminar-organizacion")]
        public async Task<IHttpActionResult> EliminarOrganizacion([FromBody]EliminarOrganizacion cmd)
        {
            await this.usuariosService.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("restaurar-organizacion")]
        public async Task<IHttpActionResult> RestaurarOrganizacion([FromBody]RestaurarOrganizacion cmd)
        {
            await this.usuariosService.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("agregar-usuario-a-la-organizacion")]
        public async Task<IHttpActionResult> AgregarUsuarioALaOrganizacion([FromBody]AgregarUsuarioALaOrganizacion command)
        {
            await this.usuariosService.HandleAsync(command.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("remover-usuario-de-organizacion")]
        public async Task<IHttpActionResult> RemoverUsuarioDeOrganizacion([FromBody]RemoverUsuarioDeOrganizacion cmd)
        {
            await this.usuariosService.HandleAsync(cmd.ConFirma(this.ActionContext));
            return this.Ok();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("otorgar-permiso")]
        public async Task<IHttpActionResult> OtorgarPermiso([FromBody]OtorgarPermiso cmd)
        {
            cmd.Autor = this.usuariosService.GetCurrentUser(this.ActionContext.GetToken());
            if (await this.usuariosService.HandleAsync(cmd.ConFirma(this.ActionContext)))
                return this.Ok();
            return this.Unauthorized();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpPost]
        [Route("retirar-permiso")]
        public async Task<IHttpActionResult> RetirarPermiso([FromBody]RetirarPermiso cmd)
        {
            cmd.Autor = this.usuariosService.GetCurrentUser(this.ActionContext.GetToken());
            if (await this.usuariosService.HandleAsync(cmd.ConFirma(this.ActionContext)))
                return this.Ok();
            return this.Unauthorized();
        }
    }
}
