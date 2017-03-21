using Agrobook.Domain.Usuarios;
using System;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class LoginClient : ClientBase<LoginClient>
    {
        private readonly string prefix = "login/";

        public LoginClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider)
        { }

        public async Task<LoginResult> TryLoginAsync(string userName, string password)
        {
            return await base.Post<LoginResult>(
                this.prefix + "try-login",
                $"{{ 'username':'{userName}', 'password':'{password}'}}");
        }
    }
}
