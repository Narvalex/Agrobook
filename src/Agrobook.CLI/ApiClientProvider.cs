using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.CLI
{
    public class ApiClientProvider
    {
        private readonly string hostUri;
        public string AccessToken { get; }

        public ApiClientProvider(string hostUri)
        {
            this.hostUri = hostUri;
        }

        public async Task<Dictionary<string, string>> GetTokenDictionary(string userName, string password)
        {
            HttpResponseMessage response;
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", password)
            };

            var content = new FormUrlEncodedContent(pairs);

            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), "Token");
                response = await client.PostAsync(tokenEndpoint, content);
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new Exception($"Error: {response.Content}");

            var tokenDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
            return tokenDictionary;
        }
    }
}
