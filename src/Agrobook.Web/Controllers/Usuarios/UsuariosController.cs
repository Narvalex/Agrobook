using Agrobook.Client.Usuarios;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Usuarios
{
    [RoutePrefix("app/usuarios")]
    public class UsuariosController : ApiController
    {


        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public IHttpActionResult CrearNuevoUsuario(UsuarioDto dto)
        {
            return this.Ok();
        }
    }
}