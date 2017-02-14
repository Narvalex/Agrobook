using System.Web.Http;

namespace Agrobook.Server.Login
{
    [RoutePrefix("login")]
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("try")]
        public IHttpActionResult Login([FromBody]dynamic value)
        {
            // Source: http://stackoverflow.com/questions/13120971/how-to-get-json-post-values-with-asp-net-webapi
            string username = value.username.Value;
            string password = value.password.Value;

            return this.Ok();
        }
    }
}
