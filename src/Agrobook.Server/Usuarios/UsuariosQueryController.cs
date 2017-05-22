using Agrobook.Domain.Usuarios.Login;
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

        [HttpGet]
        [Route("info-basica/{usuario}")]
        public async Task<IHttpActionResult> ObtenerInfoBasica([FromUri] string usuario)
        {
            var dto = await this.service.ObtenerUsuarioInfoBasica(usuario);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("organizaciones")]
        public async Task<IHttpActionResult> ObtenerOrganizaciones()
        {
            var dto = await this.service.ObtenerOrganizaciones();
            return this.Ok(dto);
        }
    }
}
