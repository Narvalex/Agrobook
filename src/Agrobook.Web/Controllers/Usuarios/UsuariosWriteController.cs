using System.Web.Http;

namespace Agrobook.Web.Controllers.Usuarios
{
    [RoutePrefix("usuarios")]
    public class UsuariosWriteController : ApiController
    {
        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public IHttpActionResult CrearNuevoUsuario(UsuarioDto dto)
        {
            return this.Ok();
        }
    }
}