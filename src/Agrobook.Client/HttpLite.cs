using Agrobook.Core;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Client
{
    public class HttpLite
    {
        private readonly string hostUri;

        public HttpLite(string hostUri)
        {
            Ensure.NotNullOrWhiteSpace(hostUri, nameof(hostUri));

            this.hostUri = hostUri;
        }

        public async Task<TResult> Post<TResult>(string uri, string jsonContent)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), uri);
                var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                response = await client.PostAsync(tokenEndpoint, stringContent);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on posting to {uri}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsAsync<TResult>();
            return responseContent;
        }

        public async Task<TResult> Post<TContent, TResult>(string uri, TContent content)
        {
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), uri);
                response = await client.PostAsJsonAsync<TContent>(tokenEndpoint.AbsoluteUri, content);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on posting to {uri}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");

            var responseContent = await response.Content.ReadAsAsync<TResult>();
            return responseContent;
        }
    }
}
