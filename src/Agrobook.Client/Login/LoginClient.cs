using Agrobook.Core;
using Agrobook.Domain.Usuarios;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class LoginClient
    {
        private readonly string hostUri;
        private readonly IJsonSerializer serializer;

        public LoginClient(string hostUri, IJsonSerializer serializer)
        {
            Ensure.NotNullOrWhiteSpace(hostUri, nameof(hostUri));
            Ensure.NotNull(serializer, nameof(serializer));

            this.hostUri = hostUri;
            this.serializer = serializer;
        }

        public async Task<LoginResult> TryLoginAsync(string userName, string password)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), "login/try-login");
                var stringContent = new StringContent($"{{ 'username':'{userName}', 'password':'{password}'}}", Encoding.UTF8, "application/json");
                response = await client.PostAsync(tokenEndpoint, stringContent);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on login. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = this.serializer.Deserialize<LoginResult>(responseContent);
            return result;
        }
    }
}
