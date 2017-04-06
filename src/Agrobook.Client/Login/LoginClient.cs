using Agrobook.Domain.Usuarios;
using System;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class LoginClient : ClientBase
    {
        public LoginClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "login")
        { }

        public async Task<LoginResult> TryLoginAsync(string userName, string password)
        {
            return await base.Post<LoginResult>("try-login",
                $"{{ 'username':'{userName}', 'password':'{password}'}}");
        }
    }
}
