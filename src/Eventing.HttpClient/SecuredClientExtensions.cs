using System;

namespace Eventing.Client.Http
{
    public static class ClientBaseExtensions
    {
        public static T WithTokenProvider<T>(this T clientBase, Func<string> tokenProvider)
            where T : ISecuredClient
        {
            clientBase.SetupTokenProvider(tokenProvider);
            return clientBase;
        }
    }
}
