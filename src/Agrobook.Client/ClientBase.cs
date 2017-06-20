﻿using Agrobook.Core;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client
{
    public abstract class ClientBase
    {
        private readonly string prefix;
        private readonly HttpLite http;
        private Func<string> tokenProvider;

        public ClientBase(HttpLite http, Func<string> tokenProvider, string prefix = "")
        {
            Ensure.NotNull(http, nameof(http));
            if (tokenProvider == null)
                // if token provider is null, then this class will be used as thread-safe
                // which implies that for every request the client will be a new instance
                this.tokenProvider = () => null;

            this.http = http;
            this.prefix = prefix + "/";
        }

        protected async Task<TResult> Post<TResult>(string uri, string jsonContent)
        {
            return await this.http.Post<TResult>(this.BuildUri(uri), jsonContent, this.tokenProvider.Invoke());
        }

        protected async Task<TResult> Post<TContent, TResult>(string uri, TContent payload)
        {
            return await this.http.Post<TContent, TResult>(this.BuildUri(uri), payload, this.tokenProvider.Invoke());
        }

        protected async Task Post<TContent>(string uri, TContent content)
        {
            await this.http.Post<TContent>(this.BuildUri(uri), content, this.tokenProvider());
        }

        protected async Task Upload(string uri, Stream fileStream, string fileName)
        {
            await this.http.Upload(this.BuildUri(uri), fileStream, fileName, this.tokenProvider());
        }

        protected async Task<TResult> Get<TResult>(string uri)
        {
            var result = await this.http.Get<TResult>(this.BuildUri(uri), this.tokenProvider());
            return result;
        }

        public void SetupTokenProvider(Func<string> tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }

        private string BuildUri(string uri)
        {
            return this.prefix + uri;
        }
    }
}
