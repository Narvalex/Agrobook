using Agrobook.Client.Login;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Login
{
    [RoutePrefix("app/login")]
    public class LoginController : ApiController
    {
        private readonly LoginClient client = ServiceLocator.Container.ResolveSingleton<LoginClient>();

        [HttpPost]
        [Route("try-login")]
        public async Task<IHttpActionResult> TryLogin([FromBody]dynamic value)
        {
            var result = await this.client.TryLoginAsync(value.usuario.Value, value.password.Value);
            return this.Ok(result);
        }
    }
}
