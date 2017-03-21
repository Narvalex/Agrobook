using Agrobook.Core;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Client
{
    public class HttpLite
    {
        private readonly string hostUri;
        private Func<string> threadSafeTokenProvider = () => string.Empty;

        public HttpLite(string hostUri)
        {
            Ensure.NotNullOrWhiteSpace(hostUri, nameof(hostUri));

            this.hostUri = hostUri;
        }

        public async Task<TResult> Post<TResult>(string uri, string jsonContent, string token = null)
        {
            HttpResponseMessage response;
            using (var client = CreateHttpClient(token))
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

        public async Task Post<TContent>(string uri, TContent content, string token = null)
        {
            await this.TryPost<TContent>(uri, content, token);
        }

        public async Task<TResult> Post<TContent, TResult>(string uri, TContent content, string token = null)
        {
            HttpResponseMessage response = await this.TryPost(uri, content, token);

            var responseContent = await response.Content.ReadAsAsync<TResult>();
            return responseContent;
        }

        private async Task<HttpResponseMessage> TryPost<TContent>(string uri, TContent content, string token)
        {
            HttpResponseMessage response;
            using (var client = this.CreateHttpClient(token))
            {
                var tokenEndpoint = new Uri(new Uri(this.hostUri), uri);
                response = await client.PostAsJsonAsync<TContent>(tokenEndpoint.AbsoluteUri, content);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on posting to {uri}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            return response;
        }

        private HttpClient CreateHttpClient(string token)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
