using Agrobook.Core;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Agrobook.Server.Authorization
{
    public class ApplyAuthPolicyAttribute : AuthorizeAttribute
    {
        public static ITokenAuthorizationProvider AuthProvider = new NullAuthProvider();
        private readonly string[] claims = new string[0];

        public ApplyAuthPolicyAttribute(string claim)
        {
            this.claims = new string[1] { claim };
        }

        public ApplyAuthPolicyAttribute(string[] claims)
        {
            this.claims = claims;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var apiAttributes = GetAttributes<ApplyAuthPolicyAttribute>(actionContext);

            if (apiAttributes != null && apiAttributes.Any())
                base.OnAuthorization(actionContext);
        }

        public string[] Claims { get; set; }

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
                    return false;
            }

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
