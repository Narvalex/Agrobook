using Agrobook.Core;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Agrobook.Server.Authorization
{
    public class Autorizar : AuthorizeAttribute
    {
        public static ITokenAuthorizationProvider AuthProvider = new NullAuthProvider();
        private readonly string[] claims = new string[0];

        public Autorizar(string claim)
        {
            this.claims = new string[1] { claim };
        }

        public Autorizar(string[] claims)
        {
            this.claims = claims;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var apiAttributes = GetAttributes<Autorizar>(actionContext);

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
                        return AuthProvider.TryAuthorize(tokenDescriptor.Parameter, this.claims);

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
