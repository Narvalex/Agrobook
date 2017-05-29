using Agrobook.Client;
using Agrobook.Client.Usuarios;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Usuarios
{
    [RoutePrefix("app/usuarios/query")]
    public class UsuariosQueryController : ApiControllerBase
    {
        private readonly UsuariosQueryClient client;

        public UsuariosQueryController()
        {
            this.client = ServiceLocator
                            .ResolveNewOf<UsuariosQueryClient>()
                            .WithTokenProvider(this.TokenProvider);
        }

        [HttpGet]
        [Route("todos")]
        public async Task<IHttpActionResult> ObtenerListaDeTodosLosUsuarios()
        {
            var lista = await this.client.ObtenerListaDeTodosLosUsuarios();
            return this.Ok(lista);
        }

        [HttpGet]
        [Route("info-basica/{usuario}")]
        public async Task<IHttpActionResult> ObtenerInfoBasicaDeUsuario([FromUri] string usuario)
        {
            var dto = await this.client.ObtenerInfoBasicaDeUsuario(usuario);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("claims")]
        public async Task<IHttpActionResult> ObtenerClaims()
        {
            var dto = await this.client.ObtenerClaims();
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("organizaciones")]
        public async Task<IHttpActionResult> ObtenerOrganizaciones()
        {
            var dto = await this.client.ObtenerOrganizaciones();
            return this.Ok(dto);
        }
    }
}