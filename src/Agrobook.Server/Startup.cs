using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var webApiConfiguration = this.GetConfiguration();

            // Using the extension method provided by the WebApi.Owin library
            app.UseWebApi(webApiConfiguration);
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
