using Agrobook.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class AccessTokenProvider
    {
        private readonly string hostUri;

        public AccessTokenProvider(string hostUri)
        {
            Ensure.NotNullOrWhiteSpace(hostUri, nameof(hostUri));

            this.hostUri = hostUri;
        }

        public string Token { get; private set; }

        public async Task<Dictionary<string, string>> TryGetTokenDictionary(string userName, string password)
        {
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            var content = new FormUrlEncodedContent(pairs);

            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), "token");
                response = await client.PostAsync(tokenEndpoint, content);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on login attempt: {responseContent}");

            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            this.Token = deserialized["access_token"];
            return deserialized;
        }
    }
}
