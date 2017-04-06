using Agrobook.Client;
using Agrobook.Client.Usuarios;
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
                            .WithTokenProvider(this.GetToken);
        }

        [HttpPost]
        [Route("crear-nuevo-usuario")]
        public async Task<IHttpActionResult> CrearNuevoUsuario(UsuarioDto dto)
        {
            await this.client.CrearNuevoUsuario(dto);
            return this.Ok();
        }
    }
}