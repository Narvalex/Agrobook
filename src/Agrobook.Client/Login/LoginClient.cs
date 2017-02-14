using Agrobook.Core;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class LoginClient
    {
        private readonly string hostUri;

        public LoginClient(string hostUri)
        {
            Ensure.NotNullOrWhiteSpace(hostUri, nameof(hostUri));

            this.hostUri = hostUri;
        }

        public async Task TryLogin(string userName, string password)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), "login/try");
                var stringContent = new StringContent($"{{ 'username':'{userName}', 'password':'{password}'}}", Encoding.UTF8, "application/json");
                response = await client.PostAsync(tokenEndpoint, stringContent);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on login attempt: {responseContent}");
        }
    }
}
