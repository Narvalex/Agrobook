using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Agrobook.Web.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        protected string TokenProvider()
        {
            IEnumerable<string> values;
            return this.ActionContext
                        .Request
                        .Headers
                        .TryGetValues("Authorization", out values)
                        ? values.First()
                        : null;
        }
    }
}