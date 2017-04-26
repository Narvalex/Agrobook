using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Server.Filters;
using System;
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
        public async Task<IHttpActionResult> ActualizarPerfil()
        {
            throw new NotImplementedException();

        }
    }
}
