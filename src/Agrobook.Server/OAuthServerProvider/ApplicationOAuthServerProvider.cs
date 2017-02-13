using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Agrobook.Server.OAuthServerProvider
{
    /// <summary>
    /// Check this post: http://johnatten.com/2015/01/19/asp-net-web-api-understanding-owinkatana-authenticationauthorization-part-i-concepts/
    /// </summary>
    public class ApplicationOAuthServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            // This call is required...
            // but we're not using client authentication, so validate and move on...
            await Task.FromResult(context.Validated());
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // DEMO ONLY: Pretend we are doing some sort of REAL checking here:
            //if (context.Password != "password")
            //{
            //    context.SetError("invalid_grant", "The user name or password is incorrect.");
            //    context.Rejected();
            //    return;
            //}
            // Create o retrieve a ClaimsIdentity to represent the 
            // Authenticated user:
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user_name", context.UserName));

            // Identity info will ultimately be encoded into an Access Token 
            // as a result of this call
            await Task.FromResult(context.Validated(identity));
        }
    }
}
