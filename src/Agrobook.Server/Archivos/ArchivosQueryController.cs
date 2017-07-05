using Agrobook.Domain.Archivos.Services;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Archivos
{
    [RoutePrefix("archivos/query")]
    public class ArchivosQueryController : ApiController
    {
        private readonly ArchivosQueryService service = ServiceLocator.ResolveSingleton<ArchivosQueryService>();
        private readonly IArchivosDelProductorFileManager fileManager = ServiceLocator.ResolveSingleton<IArchivosDelProductorFileManager>();

        [HttpGet]
        [Route("productores")]
        public async Task<IHttpActionResult> ObtenerProductores()
        {
            var dto = await this.service.ObtenerProductores();
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("archivos-del-productor/{idProductor}")]
        public async Task<IHttpActionResult> ObtenerArchivosDelProductor([FromUri]string idProductor)
        {
            var dto = await this.service.ObtenerArchivosDelProductor(idProductor);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("download/{idProductor}/{nombreArchivo}/{extension}")]
        public HttpResponseMessage Download([FromUri]string idProductor, [FromUri] string nombreArchivo, [FromUri] string extension)
        {
            //if (!File.Exists(localFilePath))
            //{
            //    result = Request.CreateResponse(HttpStatusCode.Gone);
            //}

            var result = this.Request.CreateResponse(HttpStatusCode.OK);
            var fileStream = this.fileManager.GetFile(idProductor, nombreArchivo, extension);
            result.Content = new StreamContent(fileStream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = $"{nombreArchivo}.{extension}";

            return result;
        }
    }
}
