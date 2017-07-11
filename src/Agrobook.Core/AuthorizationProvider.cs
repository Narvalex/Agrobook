using System;

namespace Agrobook.Core
{
    public interface ITokenAuthorizationProvider
    {
        bool TryAuthorize(string token, params string[] claimsRequired);

        string[] GetClaims(string token);
    }

    public class NullAuthProvider : ITokenAuthorizationProvider
    {
        public string[] GetClaims(string token)
        {
            throw new NotImplementedException();
        }

        public bool TryAuthorize(string token, params string[] claimsRequired) => true;
    }
}
