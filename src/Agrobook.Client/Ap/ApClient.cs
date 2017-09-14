using Eventing.Client.Http;
using System;

namespace Agrobook.Client.Ap
{
    public class ApClient : ClientBase
    {
        public ApClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "ap")
        { }
    }
}
