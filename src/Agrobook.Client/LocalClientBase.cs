using System;

namespace Agrobook.Client
{
    public abstract class LocalClientBase
    {
        protected Func<string> tokenProvider;

        public void SetupTokenProvider(Func<string> tokenProvider)
        {
            this.tokenProvider = tokenProvider;
        }
    }
}
