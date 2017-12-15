using System;

namespace Eventing.Client
{
    public interface ISecuredClient
    {
        void SetupTokenProvider(Func<string> tokenProvider);
    }
}
