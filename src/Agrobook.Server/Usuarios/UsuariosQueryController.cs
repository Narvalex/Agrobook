using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimsDefs;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios/query")]
    public class UsuariosQueryController : ApiController
    {
        private readonly UsuariosQueryService service = ServiceLocator.ResolveSingleton<UsuariosQueryService>();
        private readonly UsuariosService usuariosService = ServiceLocator.ResolveSingleton<UsuariosService>();
        private readonly OrganizacionesQueryService organizacionesQueryService = ServiceLocator.ResolveSingleton<OrganizacionesQueryService>();

        [Autorizar(Roles.Gerente)]
        [HttpGet]
        [Route("todos")]
        public async Task<IHttpActionResult> ObtenerTodosLosUsuarios()
        {
            var lista = await this.service.ObtenerTodosLosUsuarios();
            return this.Ok(lista);
        }

        [HttpGet]
        [Route("info-basica/{usuario}")]
        public async Task<IHttpActionResult> ObtenerInfoBasica([FromUri] string usuario)
        {
            var dto = await this.service.ObtenerUsuarioInfoBasica(usuario);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("claims")]
        public async Task<IHttpActionResult> ObtenerClaims()
        {
            var claims = await this.usuariosService.ObtenerListaDeClaimsDisponiblesParaElUsuario(this.ActionContext.GetToken());
            return this.Ok(claims);
        }

        [Autorizar(Roles.Gerente, Permisos.AdministrarOrganizaciones)]
        [HttpGet]
        [Route("organizaciones")]
        public async Task<IHttpActionResult> ObtenerOrganizaciones()
        {
            var dto = await this.organizacionesQueryService.ObtenerOrganizaciones();
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("organizaciones/{usuarioId}")]
        public async Task<IHttpActionResult> ObtenerOrganizaciones([FromUri]string usuarioId)
        {
            var dto = await this.organizacionesQueryService.ObtenerOrganizaciones(usuarioId);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("organizaciones-marcadas-del-usuario/{usuarioId}")]
        public async Task<IHttpActionResult> ObtenerOrganizacionesMarcadasDelUsuario([FromUri]string usuarioId)
        {
            var dto = await this.organizacionesQueryService.ObtenerOrganizacionesMarcadasDelUsuario(usuarioId);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("grupos/{idOrganizacion}")]
        public async Task<IHttpActionResult> ObtenerGrupos([FromUri]string idOrganizacion)
        {
            var dto = await this.organizacionesQueryService.ObtenerGrupos(idOrganizacion);
            return this.Ok(dto);
        }
    }
}
