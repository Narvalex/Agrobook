using Agrobook.Server.OAuthServerProvider;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Web.Http;

namespace Agrobook.Server
{
    /// <summary>
    /// Startup for the http server. Thanks to John Atten tutorial: http://johnatten.com/2015/01/11/asp-net-web-api-2-2-create-a-self-hosted-owin-based-web-api-from-scratch/
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Required by Katana
        /// </summary>
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);

            var webApiConfiguration = this.GetConfiguration();

            // Using the extension method provided by the WebApi.Owin library
            app.UseWebApi(webApiConfiguration);
        }

        private void ConfigureAuth(IAppBuilder app)
        {
            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),

                // Only do this for demo!!
                AllowInsecureHttp = true
            };
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private HttpConfiguration GetConfiguration()
        {
            var config = new HttpConfiguration();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            // supporting attributed routes :D
            config.MapHttpAttributeRoutes();

            return config;
        }
    }
}
