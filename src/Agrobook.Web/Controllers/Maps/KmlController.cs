using System.Web.Http;

namespace Agrobook.Web.Controllers.Maps
{
    [RoutePrefix("kml")]
    public class KmlController : ApiController
    {
        [HttpGet]
        [Route("sample")]
        public IHttpActionResult Sample()
        {
            return this.Ok();
        }
    }
}