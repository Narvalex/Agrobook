using Agrobook.Core;
using System;
using System.Threading.Tasks;

namespace Agrobook.Client
{
    public abstract class ClientBase<T> where T : ClientBase<T>
    {
        private readonly HttpLite http;
        private Func<string> tokenProvider;

        public ClientBase(HttpLite http, Func<string> tokenProvider)
        {
            Ensure.NotNull(http, nameof(http));
            if (tokenProvider == null)
                // if token provider is null, then this class will be used as thread-safe
                // which implies that for every request the client will be a new instance
                this.tokenProvider = () => null;

            this.http = http;
        }

        protected async Task<TResult> Post<TResult>(string uri, string jsonContent)
        {
            return await this.http.Post<TResult>(uri, jsonContent, this.tokenProvider.Invoke());
        }

        protected async Task Post<TContent>(string uri, TContent content)
        {
            await this.http.Post<TContent>(uri, content, this.tokenProvider());
        }

        public T SetupTokenProvider(Func<string> tokenProvider)
        {
            this.tokenProvider = tokenProvider;
            return (T)this;
        }
    }
}
