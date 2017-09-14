using Agrobook.Domain.Ap.Services;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    [RoutePrefix("ap")]
    public class ApController : ApiController
    {
        private readonly ApService service = ServiceLocator.ResolveSingleton<ApService>();


    }
}
