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
        private readonly IFileWriter fileManager = ServiceLocator.ResolveSingleton<IFileWriter>();

        [HttpGet]
        [Route("coleccion/{idColeccion}")]
        public async Task<IHttpActionResult> ObtenerListaDeArchivos([FromUri]string idColeccion)
        {
            var lista = await this.queryService.ObtenerArchivos(idColeccion);
            return this.Ok(lista);
        }

        [HttpGet]
        [Route("download/{idColeccion}/{nombreArchivo}/{usuario}")]
        public async Task<HttpResponseMessage> Download([FromUri]string idColeccion, [FromUri] string nombreArchivo, [FromUri] string usuario)
        {
            //if (!File.Exists(localFilePath))
            //{
            //    result = Request.CreateResponse(HttpStatusCode.Gone);
            //}

            // Validar aqui el usuario

            var response = this.PrepareFileResponse(idColeccion, nombreArchivo);

            await this.service.HandleAsync(new RegistrarDescargaExitosa(new Firma(usuario, DateTime.Now), idColeccion, nombreArchivo));

            return response;
        }

        [HttpGet]
        [Route("preview/{idColeccion}/{nombreArchivo}/{usuario}")]
        public HttpResponseMessage Preview([FromUri]string idColeccion, [FromUri] string nombreArchivo, [FromUri] string usuario)
        {
            // Validar aqui el usuario
            // Validar que sea imagen
            var response = this.PrepareFileResponse(idColeccion, nombreArchivo);

            return response;
        }

        private HttpResponseMessage PrepareFileResponse(string idColeccion, string nombreArchivo)
        {
            var result = this.Request.CreateResponse(HttpStatusCode.OK);
            var fileStream = this.fileManager.GetFile(idColeccion, nombreArchivo);
            result.Content = new StreamContent(fileStream);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = $"{nombreArchivo}";
            return result;
        }

        /******************************************************************
         * This is interesting to show a picture in the browser
         ******************************************************************/
        //[HttpGet]
        //[Route("preview/{idProductor}/{nombreArchivo}")]
        //public HttpResponseMessage Preview([FromUri]string idProductor, [FromUri] string nombreArchivo)
        //{
        // VALIDAR AQUI QUE NO SEA UN ARCHIVO GRANDE
        // TODO: validar que no sea imagen. Si es imagen, hay que rechazar...
        //if (true)
        //{
        //    return this.Request.CreateResponse(HttpStatusCode.BadRequest);
        //}

        //var response = this.PrepareFileResponse(idProductor, nombreArchivo);
        //return response;
        //}
    }
}
