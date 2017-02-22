using Agrobook.Core;
using System.Web.Http;

namespace Agrobook.Server
{
    public abstract class ApiControllerBase : ApiController
    {
        protected readonly ISimpleContainer container = ServiceLocator.Container;
    }
}
