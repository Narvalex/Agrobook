using Agrobook.Core;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Agrobook.Server.Filters
{
    public class AutorizarAttribute : AuthorizeAttribute
    {
        private static ITokenAuthorizationProvider _authProvider = new NullAuthProvider();
        private readonly string[] claims = new string[0];

        public AutorizarAttribute(string claim)
        {
            this.claims = new string[1] { claim };
        }

        public AutorizarAttribute(params string[] claims)
        {
            this.claims = claims;
        }

        public static void SetTokenAuthProvider(ITokenAuthorizationProvider tokenAuthProvider)
        {
            _authProvider = tokenAuthProvider;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var apiAttributes = GetAttributes<AutorizarAttribute>(actionContext);

            if (apiAttributes != null && apiAttributes.Any())
                base.OnAuthorization(actionContext);
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (this.GetAttributes<AllowAnonymousAttribute>(actionContext).Any())
                return true;

            IEnumerable<string> values;
            if (actionContext.Request.Headers.TryGetValues("Authorization", out values))
            {
                if (values.Any(v =>
                {
                    AuthenticationHeaderValue tokenDescriptor;
                    if (AuthenticationHeaderValue.TryParse(v, out tokenDescriptor))
                        return _authProvider.TryAuthorize(tokenDescriptor.Parameter, this.claims);

                    return false;
                }))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private IEnumerable<T> GetAttributes<T>(HttpActionContext actionContext) where T : class
        {
            return actionContext
                    .ActionDescriptor
                    .GetCustomAttributes<T>(true)
                    .Concat(actionContext
                            .ActionDescriptor
                            .ControllerDescriptor
                            .GetCustomAttributes<T>());
        }
    }
}
