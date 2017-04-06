﻿using Agrobook.Domain.Usuarios.Login;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Server.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Usuarios
{
    [Autorizar(Claims.Roles.Admin)]
    [RoutePrefix("usuarios/query")]
    public class UsuariosQueryController : ApiController
    {
        private readonly UsuariosQueryService service = ServiceLocator.ResolveSingleton<UsuariosQueryService>();

        [HttpGet]
        [Route("todos")]
        public async Task<IHttpActionResult> ObtenerTodosLosUsuarios()
        {
            var lista = await this.service.ObtenerTodosLosUsuarios();
            return this.Ok(lista);
        }
    }
}