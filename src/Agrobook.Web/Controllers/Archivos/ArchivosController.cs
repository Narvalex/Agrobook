using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net.Http;

namespace Agrobook.Web.Controllers.Archivos
{
    [RoutePrefix("app/archivos")]
    public class ArchivosController : ApiControllerBase
    {
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            /*
             * HTML CSS JAVASCRIPT: http://jsfiddle.net/vishalvasani/4hqVu/
             * Another one: http://www.uncorkedstudios.com/blog/multipartformdata-file-upload-with-angularjs/
             * 
             * Help: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/sending-html-form-data-part-2
             * Help2: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/sending-html-form-data-part-2
             * 
             */
            if (!this.Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);

            return this.Ok();
        }
    }
}