using Eventing.Log;
using System.Web.Http.Filters;

namespace Agrobook.Server.Filters
{
    public class GlobalErrorAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogManager.GlobalLogger.Error(actionExecutedContext.Exception, $"Error en: {actionExecutedContext.Request.RequestUri}");
        }
    }
}
