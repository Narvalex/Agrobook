﻿using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server.Usuarios
{
    [RoutePrefix("usuarios/query")]
    public class UsuariosQueryController : ApiController
    {
        private readonly UsuariosQueryService usuarioQueryService = ServiceLocator.ResolveSingleton<UsuariosQueryService>();
        private readonly UsuariosService usuariosService = ServiceLocator.ResolveSingleton<UsuariosService>();
        private readonly OrganizacionesQueryService organizacionesQueryService = ServiceLocator.ResolveSingleton<OrganizacionesQueryService>();

        [Autorizar(Roles.Gerente)]
        [HttpGet]
        [Route("todos")]
        public async Task<IHttpActionResult> ObtenerTodosLosUsuarios()
        {
            var lista = await this.usuarioQueryService.ObtenerTodosLosUsuarios();
            return this.Ok(lista);
        }

        [HttpGet]
        [Route("info-basica/{usuario}")]
        public async Task<IHttpActionResult> ObtenerInfoBasica([FromUri] string usuario)
        {
            var dto = await this.usuarioQueryService.ObtenerUsuarioInfoBasica(usuario);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("claims")]
        public async Task<IHttpActionResult> ObtenerClaims()
        {
            var claims = await this.usuariosService.ObtenerListaDeClaimsDisponiblesParaElUsuario(this.ActionContext.GetToken());
            return this.Ok(claims);
        }

        [HttpGet]
        [Route("claims/{idUsuario}")]
        public async Task<IHttpActionResult> ObtenerClaims([FromUri] string idUsuario)
        {
            var claims = await this.usuariosService.ObtenerClaimsDelUsuario(idUsuario);
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
        [Route("grupos/{idOrganizacion}/{idUsuario}")]
        public async Task<IHttpActionResult> ObtenerGrupos([FromUri]string idOrganizacion, [FromUri]string idUsuario)
        {
            var dto = await this.organizacionesQueryService.ObtenerGrupos(idOrganizacion, idUsuario);
            return this.Ok(dto);
        }
    }
}
