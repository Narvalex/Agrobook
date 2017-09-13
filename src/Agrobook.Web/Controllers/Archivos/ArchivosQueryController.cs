using Agrobook.Client.Archivos;
using Eventing.Client.Http;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Archivos
{
    [RoutePrefix("app/archivos/query")]
    public class ArchivosQueryController : ApiControllerBase
    {
        private readonly ArchivosQueryClient client;

        public ArchivosQueryController()
        {
            this.client = ServiceLocator.ResolveNewOf<ArchivosQueryClient>().WithTokenProvider(this.TokenProvider);
        }

        //[HttpGet]
        //[Route("productores")]
        //public async Task<IHttpActionResult> ObtenerProductores()
        //{
        //    var dto = await this.client.ObtenerProductores();
        //    return this.Ok(dto);
        //}

        //[HttpGet]
        //[Route("archivos-del-productor/{idProductor}")]
        //public async Task<IHttpActionResult> ObtenerArchivosDelProductor([FromUri]string idProductor)
        //{
        //    var dto = await this.client.ObtenerArchivosDelProductor(idProductor);
        //    return this.Ok(dto);
        //}

        [HttpGet]
        [Route("coleccion/{idColeccion}")]
        public async Task<IHttpActionResult> ObtenerListaDeArchivos([FromUri]string idColeccion)
        {
            var list = await this.client.ObtenerListaDeArchivos(idColeccion);
            return this.Ok(list);
        }

        [HttpGet]
        [Route("download/{idColeccion}/{nombreArchivo}/{usuario}")]
        public async Task<HttpResponseMessage> Download([FromUri]string idColeccion, [FromUri] string nombreArchivo, [FromUri]string usuario) // security bug here
        {
            var stream = await this.client.Download(idColeccion, nombreArchivo, usuario);

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = nombreArchivo;

            return response;
        }

        [HttpGet]
        [Route("file-icon/{idProductor}/{archivo}")]
        public async Task<HttpResponseMessage> GetFileIcon([FromUri]string idProductor, [FromUri]string archivo)
        {
            // TODO: extract extension
            var extension = default(string);

            byte[] byteArray;
            extension = extension.ToLowerInvariant();
            if (extension != "jpg"
                && extension != "png"
                && extension != "jpeg")
            {
                var path = HttpContext.Current.Server.MapPath("~/app/assets/img/fileIcons/file.png");
                byteArray = File.ReadAllBytes(path);
            }
            else
            {
                var stream = await this.client.Download(idProductor, archivo, extension);
                byteArray = await ReadToByte(stream);
            }

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(byteArray);
            await response.Content.LoadIntoBufferAsync(byteArray.Length);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            return response;
        }

        private static async Task<byte[]> ReadToByte(Stream input)
        {
            using (var memoryStream = new MemoryStream())
            {
                await input.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}