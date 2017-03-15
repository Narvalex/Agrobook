using Agrobook.Core;
using Agrobook.Domain.Usuarios;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class LoginClient
    {
        private readonly HttpLite http;

        public LoginClient(HttpLite http)
        {
            Ensure.NotNull(http, nameof(http));

            this.http = http;
        }

        public async Task<LoginResult> TryLoginAsync(string userName, string password)
        {
            return await this.http.Post<LoginResult>(
                "login/try-login",
                $"{{ 'username':'{userName}', 'password':'{password}'}}");
        }
    }
}
