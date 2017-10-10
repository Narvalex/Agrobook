using Agrobook.Client.Usuarios;
using Agrobook.Domain.Usuarios;
using Eventing.Client.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Usuarios
{
    [RoutePrefix("app/usuarios")]
    public class UsuariosController : ApiControllerBase
    {
        private readonly UsuariosClient client;

        public UsuariosController()
        {
            this.client = ServiceLocator
                            .ResolveNewOf<UsuariosClient>()
                            .WithTokenProvider(this.TokenProvider);
        }

        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuario(UsuarioDto dto)
        {
            await this.client.CrearNuevoUsuario(dto);
            return this.Ok();
        }

        [HttpPost]
        [Route("actualizar-perfil")]
        public async Task<IHttpActionResult> ActualizarPerfil(ActualizarPerfilDto dto)
        {
            await this.client.ActualizarPerfil(dto);
            return this.Ok();
        }

        [HttpPost]
        [Route("resetear-password/{usuario}")]
        public async Task<IHttpActionResult> ResetearPassword([FromUri]string usuario)
        {
            await this.client.ResetearPassword(usuario);
            return this.Ok();
        }

        [HttpPost]
        [Route("crear-nueva-organizacion/{nombreOrg}")]
        public async Task<IHttpActionResult> CrearNuevaOrganizacion([FromUri]string nombreOrg)
        {
            var org = await this.client.CrearNuevaOrganización(nombreOrg);
            return this.Ok(org);
        }

        [HttpPost]
        [Route("crear-nuevo-grupo/{idOrg}/{displayGrupoName}")]
        public async Task<IHttpActionResult> CrearNuevoGrupo([FromUri]string idOrg, [FromUri]string displayGrupoName)
        {
            var grupo = await this.client.CrearNuevoGrupo(idOrg, displayGrupoName);
            return this.Ok(grupo);
        }

        [HttpPost]
        [Route("agregar-usuario-a-la-organizacion/{idUsuario}/{idOrganizacion}")]
        public async Task<IHttpActionResult> AgregarUsuarioALaOrganizacion([FromUri]string idUsuario, [FromUri]string idOrganizacion)
        {
            await this.client.AgregarUsuarioALaOrganizacion(idUsuario, idOrganizacion);
            return this.Ok();
        }

        [HttpPost]
        [Route("agregar-usuario-a-grupo/{idUsuario}/{idOrganizacion}/{idGrupo}")]
        public async Task<IHttpActionResult> AgregarUsuarioAGrupo([FromUri]string idUsuario, [FromUri]string idOrganizacion, [FromUri]string idGrupo)
        {
            await this.client.AgregarUsuarioAGrupo(idUsuario, idOrganizacion, idGrupo);
            return this.Ok();
        }

        [HttpPost]
        [Route("remover-usuario-de-un-grupo/{idUsuario}/{idOrganizacion}/{idGrupo}")]
        public async Task<IHttpActionResult> RemoverUsuarioDeUnGrupo([FromUri]string idUsuario, [FromUri]string idOrganizacion, [FromUri]string idGrupo)
        {
            await this.client.RemoverUsuarioDeUnGrupo(idUsuario, idOrganizacion, idGrupo);
            return this.Ok();
        }

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> RemoverUsuarioDeOrganizacion([FromBody]RemoverUsuarioDeOrganizacion cmd)
        {
            await this.client.RemoverUsuarioDeOrganizacion(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("otorgar-permiso")]// ?usuario=${idUsuario}&permiso=${permiso}`
        public async Task<IHttpActionResult> OtorgarPermiso([FromUri]string usuario, [FromUri]string permiso)
        {
            await this.client.OtorgarPermisoAUsuario(usuario, permiso);
            return this.Ok();
        }

        [HttpPost]
        [Route("retirar-permiso")]
        public async Task<IHttpActionResult> RetirarPermiso([FromUri]string usuario, [FromUri]string permiso)
        {
            await this.client.RetirarPermiso(usuario, permiso);
            return this.Ok();
        }
    }
}