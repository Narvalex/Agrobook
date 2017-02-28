using Newtonsoft.Json.Serialization;
using System;
using System.Web.Http;

namespace Agrobook.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object _, EventArgs __)
        {
            GlobalConfiguration.Configure(config =>
            {
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional });

                config.MapHttpAttributeRoutes();

                config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
        }
    }
}