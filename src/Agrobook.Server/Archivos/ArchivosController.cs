using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Archivos
{
    [RoutePrefix("archivos")]
    public class ArchivosController : ApiController
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!this.Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);

            var streamProvider = await this.Request.Content.ReadAsMultipartAsync();
            var content = streamProvider.Contents.First();

            var fileName = content.Headers.ContentDisposition.FileName;
            using (var stream = await content.ReadAsStreamAsync())
            {
                return this.Ok();
            }
        }
    }
}
