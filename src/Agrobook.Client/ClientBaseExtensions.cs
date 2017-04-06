using System;

namespace Agrobook.Client
{
    public static class ClientBaseExtensions
    {
        public static T WithTokenProvider<T>(this T clientBase, Func<string> tokenProvider) 
            where T : ClientBase
        {
            clientBase.SetupTokenProvider(tokenProvider);
            return clientBase;
        }
    }
}
