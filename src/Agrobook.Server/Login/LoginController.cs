using Agrobook.Common;
using Agrobook.Domain.Usuarios;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Login
{
    [RoutePrefix("login")]
    public class LoginController : ApiController
    {
        private readonly IDateTimeProvider dateTime = ServiceLocator.ResolveSingleton<IDateTimeProvider>();
        private readonly UsuariosService usuariosService = ServiceLocator.ResolveSingleton<UsuariosService>();

        [HttpPost]
        [Route("try-login")]
        public async Task<IHttpActionResult> TryLogin([FromBody]dynamic value)
        {
            // Source: http://stackoverflow.com/questions/13120971/how-to-get-json-post-values-with-asp-net-webapi
            string username = value.username.Value;
            string password = value.password.Value;

            var result = await this.usuariosService.HandleAsync(new IniciarSesion(username, password, this.dateTime.Now));

            return this.Ok(result);
        }
    }
}
