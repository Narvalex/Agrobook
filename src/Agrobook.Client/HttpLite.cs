using Agrobook.Core;
using System;
using System.Globalization;
using System.IO;
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

        public async Task<TResult> Get<TResult>(string uri, string token = null)
        {
            HttpResponseMessage response;
            using (var client = this.CreateHttpClient(token))
            {
                var endpoint = new Uri(new Uri(this.hostUri), uri);
                response = await client.GetAsync(endpoint.AbsoluteUri);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on posting to {uri}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            var result = await response.Content.ReadAsAsync<TResult>();
            return result;
        }

        public async Task<TResult> Post<TResult>(string uri, string jsonContent, string token = null)
        {
            HttpResponseMessage response;
            using (var client = this.CreateHttpClient(token))
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

        // to build a stream from a byte array = new MemoryStream(byteArray);
        //
        public async Task Upload(string uri, Stream fileStream, string fileName, string metadatos, string token = null)
        {
            // Reference: https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload?noredirect=1&lq=1

            using (var client = this.CreateHttpClient(token, false))
            {
                using (var content = new MultipartFormDataContent($"Upload----{DateTime.Now.ToString(CultureInfo.InvariantCulture)}"))
                {
                    content.Add(new StreamContent(fileStream), "uploadedFile", fileName);
                    content.Add(new StringContent(metadatos));

                    var endpoint = new Uri(new Uri(this.hostUri), uri);
                    var url = endpoint.AbsoluteUri;
                    using (var response = await client.PostAsync(url, content))
                    {
                        if (!response.IsSuccessStatusCode)
                            throw new Exception($"Error on posting to {uri}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
                    }
                }
            }
        }

        public async Task Post<TContent>(string uri, TContent content, string token = null)
        {
            await this.TryPostAsJson<TContent>(uri, content, token);
        }

        public async Task<TResult> Post<TContent, TResult>(string uri, TContent content, string token = null)
        {
            HttpResponseMessage response = await this.TryPostAsJson(uri, content, token);

            var responseContent = await response.Content.ReadAsAsync<TResult>();
            return responseContent;
        }

        private async Task<HttpResponseMessage> TryPostAsJson<TContent>(string uri, TContent content, string token)
        {
            HttpResponseMessage response;
            using (var client = this.CreateHttpClient(token))
            {
                var endpoint = new Uri(new Uri(this.hostUri), uri);
                response = await client.PostAsJsonAsync<TContent>(endpoint.AbsoluteUri, content);
            }

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Error on posting to {uri}. Status Code: {response.StatusCode}. Reason: {response.ReasonPhrase}");
            return response;
        }

        private HttpClient CreateHttpClient(string token, bool isJson = true)
        {
            var client = new HttpClient();
            if (isJson)
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (token != null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
