using Agrobook.Client;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace Agrobook.Web.Filters
{
    // this can be used in a per controller stuff
    // based on: https://docs.microsoft.com/en-us/aspnet/web-api/overview/error-handling/exception-handling
    //
    public class RemoteUnauthrorizedExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is RemoteUnauthrorizedResponseException)
            {
                actionExecutedContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}