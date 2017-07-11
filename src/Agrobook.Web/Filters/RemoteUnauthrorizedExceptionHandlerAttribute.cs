using System.Web.Http.Filters;

namespace Agrobook.Web.Filters
{
    // this can be used in a per controller stuff
    public class RemoteUnauthrorizedExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnException(actionExecutedContext);
        }
    }
}