using System.IO;
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
                var formattedFileName = @"\" + new string(fileName.Trim().Where(c => c != '"').ToArray());

                var path = @"\files";
                var current = Directory.GetCurrentDirectory();
                var fullPath = current + path + formattedFileName;
                using (var fileStream = File.Create(fullPath))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
                return this.Ok();
            }
        }
    }
}
