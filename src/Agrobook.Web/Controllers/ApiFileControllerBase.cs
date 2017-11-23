using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Agrobook.Web.Controllers
{
    public abstract class ApiFileControllerBase : ApiControllerBase
    {
        protected HttpResponseMessage PrepareResponse(string nombreArchivo, System.IO.Stream stream)
        {
            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = nombreArchivo;

            return response;
        }


        /******************************************************************
         * This is interesting to show a picture in the browser
         ******************************************************************/
        //[HttpGet]
        //[Route("file-icon/{idProductor}/{archivo}")]
        //public async Task<HttpResponseMessage> GetFileIcon([FromUri]string idProductor, [FromUri]string archivo)
        //{
        //    // TODO: extract extension
        //    var extension = default(string);

        //    byte[] byteArray;
        //    extension = extension.ToLowerInvariant();
        //    if (extension != "jpg"
        //        && extension != "png"
        //        && extension != "jpeg")
        //    {
        //        var path = HttpContext.Current.Server.MapPath("~/app/assets/img/fileIcons/file.png");
        //        byteArray = File.ReadAllBytes(path);
        //    }
        //    else
        //    {
        //        var stream = await this.client.Download(idProductor, archivo, extension);
        //        byteArray = await ReadToByte(stream);
        //    }

        //    var response = this.Request.CreateResponse(HttpStatusCode.OK);
        //    response.Content = new ByteArrayContent(byteArray);
        //    await response.Content.LoadIntoBufferAsync(byteArray.Length);
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        //    return response;
        //}

        //private static async Task<byte[]> ReadToByte(Stream input)
        //{
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        await input.CopyToAsync(memoryStream);
        //        return memoryStream.ToArray();
        //    }
        //}
    }
}