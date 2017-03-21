using Agrobook.Client.Login;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Login
{
    [RoutePrefix("app/login")]
    public class LoginController : ApiControllerBase
    {
        private readonly LoginClient client;

        public LoginController()
        {
            this.client = ServiceLocator
                            .ResolveNewOf<LoginClient>()
                            .SetupTokenProvider(this.GetToken);
        }

        [HttpPost]
        [Route("try-login")]
        public async Task<IHttpActionResult> TryLogin([FromBody]dynamic value)
        {
            var result = await this.client.TryLoginAsync(value.usuario.Value, value.password.Value);
            return this.Ok(result);
        }
    }
}
