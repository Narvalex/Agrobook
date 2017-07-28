using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Common;
using System;
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
        private readonly ArchivosService service = ServiceLocator.ResolveSingleton<ArchivosService>();
        private readonly ArchivosQueryService queryService = ServiceLocator.ResolveSingleton<ArchivosQueryService>();
        private readonly IArchivosDelProductorFileManager fileManager = ServiceLocator.ResolveSingleton<IArchivosDelProductorFileManager>();

        [HttpGet]
        [Route("productores")]
        public async Task<IHttpActionResult> ObtenerProductores()
        {
            var dto = await this.queryService.ObtenerProductores();
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("archivos-del-productor/{idProductor}")]
        public async Task<IHttpActionResult> ObtenerArchivosDelProductor([FromUri]string idProductor)
        {
            var dto = await this.queryService.ObtenerArchivosDelProductor(idProductor);
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("download/{idProductor}/{nombreArchivo}/{extension}/{usuario}")]
        public async Task<HttpResponseMessage> Download([FromUri]string idProductor, [FromUri] string nombreArchivo, [FromUri] string extension, [FromUri] string usuario)
        {
            //if (!File.Exists(localFilePath))
            //{
            //    result = Request.CreateResponse(HttpStatusCode.Gone);
            //}

            var response = this.PrepareFileResponse(idProductor, nombreArchivo, extension);

            await this.service.HandleAsync(new RegistrarDescargaExitosa(new Firma(usuario, DateTime.Now), idProductor, nombreArchivo));

            return response;
        }

        [HttpGet]
        [Route("preview/{idProductor}/{nombreArchivo}/{extension}")]
        public HttpResponseMessage Preview([FromUri]string idProductor, [FromUri] string nombreArchivo, [FromUri] string extension)
        {
            // VALIDAR AQUI QUE NO SEA UN ARCHIVO GRANDE
            if (extension != "jpg"
                && extension != "JPG"
                && extension != "png"
                && extension != "PNG")
            {
                return this.Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            var response = this.PrepareFileResponse(idProductor, nombreArchivo, extension);
            return response;
        }

        private HttpResponseMessage PrepareFileResponse(string idProductor, string nombreArchivo, string extension)
        {
            var result = this.Request.CreateResponse(HttpStatusCode.OK);
            var fileStream = this.fileManager.GetFile(idProductor, nombreArchivo, extension);
            result.Content = new StreamContent(fileStream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = $"{nombreArchivo}.{extension}";
            return result;
        }
    }
}
