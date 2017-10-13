using Agrobook.Domain.Usuarios.Services;
using Agrobook.Server.Filters;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios/query")]
    public class UsuariosQueryController : BaseApiController
    {
        private readonly UsuariosQueryService usuarioQueryService = ServiceLocator.ResolveSingleton<UsuariosQueryService>();
        private readonly OrganizacionesQueryService organizacionesQueryService = ServiceLocator.ResolveSingleton<OrganizacionesQueryService>();

        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("todos")]
        public async Task<IHttpActionResult> ObtenerTodosLosUsuarios()
        {
            var claims = this.usuariosService.GetClaims(this.ActionContext.GetToken());

            if (claims.Any(x => x == Roles.Admin))
                return this.Ok(await this.usuarioQueryService.ObtenerTodosLosUsuarios());

            else if (claims.Any(x => x == Roles.Gerente))
                return this.Ok(await this.usuarioQueryService.ObtenerTodosLosUsuariosMenosAdmines());

            else if (claims.Any(x => x == Roles.Tecnico))
                return this.Ok(await this.usuarioQueryService.ObtenerTodosLosUsuariosMenosGerentesYAdmines());

            return this.BadRequest();
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor, Roles.Invitado)]
        [HttpGet]
        [Route("info-basica/{usuario}")]
        public async Task<IHttpActionResult> ObtenerInfoBasica([FromUri] string usuario)
        {
            if (!this.SolicitaSoloSusPropiosDatos(usuario)) return this.Unauthorized();

            var dto = await this.usuarioQueryService.ObtenerUsuarioInfoBasica(usuario);
            return this.Ok(dto);
        }


        [Autorizar(Roles.Gerente, Roles.Tecnico)]
        [HttpGet]
        [Route("claims")]
        public async Task<IHttpActionResult> ObtenerClaims()
        {
            var claims = await this.usuariosService.ObtenerListaDeClaimsDisponiblesParaElUsuario(this.ActionContext.GetToken());
            return this.Ok(claims);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor, Roles.Invitado)]
        [HttpGet]
        [Route("claims/{idUsuario}")]
        public async Task<IHttpActionResult> ObtenerClaims([FromUri] string idUsuario)
        {
            if (!this.SolicitaSoloSusPropiosDatos(idUsuario)) return this.Unauthorized();

            var claims = await this.usuariosService.ObtenerClaimsDelUsuario(idUsuario);
            return this.Ok(claims);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Permisos.AdministrarOrganizaciones)]
        [HttpGet]
        [Route("organizaciones")]
        public async Task<IHttpActionResult> ObtenerOrganizaciones()
        {
            var dto = await this.organizacionesQueryService.ObtenerOrganizaciones();
            return this.Ok(dto);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor, Roles.Invitado)]
        [HttpGet]
        [Route("organizaciones/{usuarioId}")]
        public async Task<IHttpActionResult> ObtenerOrganizaciones([FromUri]string usuarioId)
        {
            if (!this.SolicitaSoloSusPropiosDatos(usuarioId)) return this.Unauthorized();

            var dto = await this.organizacionesQueryService.ObtenerOrganizaciones(usuarioId);
            return this.Ok(dto);
        }

        [Autorizar(Roles.Gerente, Roles.Tecnico, Roles.Productor, Roles.Invitado)]
        [HttpGet]
        [Route("organizaciones-marcadas-del-usuario/{usuarioId}")]
        public async Task<IHttpActionResult> ObtenerOrganizacionesMarcadasDelUsuario([FromUri]string usuarioId)
        {
            if (!this.SolicitaSoloSusPropiosDatos(usuarioId)) return this.Unauthorized();

            var dto = await this.organizacionesQueryService.ObtenerOrganizacionesMarcadasDelUsuario(usuarioId);
            return this.Ok(dto);
        }
    }
}
