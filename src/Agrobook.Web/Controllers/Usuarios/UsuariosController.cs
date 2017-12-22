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
        private readonly IUsuariosClient client;

        public UsuariosController()
        {
            this.client = ServiceLocator
                            .ResolveNewOf<IUsuariosClient>()
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
        [Route("cambiar-nombre-de-organizacion")]
        public async Task<IHttpActionResult> CambiarNombreDeOrganizacion([FromBody]CambiarNombreDeOrganizacion cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("eliminar-organizacion")]
        public async Task<IHttpActionResult> EliminarOrganizacion([FromBody]EliminarOrganizacion cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-organizacion")]
        public async Task<IHttpActionResult> RestaurarOrganizacion([FromBody]RestaurarOrganizacion cmd)
        {
            await this.client.Send(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("agregar-usuario-a-la-organizacion/{idUsuario}/{idOrganizacion}")]
        public async Task<IHttpActionResult> AgregarUsuarioALaOrganizacion([FromUri]string idUsuario, [FromUri]string idOrganizacion)
        {
            await this.client.AgregarUsuarioALaOrganizacion(idUsuario, idOrganizacion);
            return this.Ok();
        }

        [HttpPost]
        [Route("remover-usuario-de-organizacion")]
        public async Task<IHttpActionResult> RemoverUsuarioDeOrganizacion([FromBody]RemoverUsuarioDeOrganizacion cmd)
        {
            await this.client.Send(cmd);
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