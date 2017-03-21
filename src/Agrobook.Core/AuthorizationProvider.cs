namespace Agrobook.Core
{
    public interface ITokenAuthorizationProvider
    {
        bool TryAuthorize(string token, params string[] claimsRequired);
    }

    public class NullAuthProvider : ITokenAuthorizationProvider
    {
        public bool TryAuthorize(string token, params string[] claimsRequired) => true;
    }
}
