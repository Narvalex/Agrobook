﻿using Agrobook.Client.Archivos;
using Agrobook.Domain.Archivos;
using Eventing.Client.Http;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Archivos
{
    [RoutePrefix("app/archivos")]
    public class ArchivosController : ApiControllerBase
    {
        private readonly IArchivosClient client;

        public ArchivosController()
        {
            this.client = ServiceLocator.ResolveNewOf<IArchivosClient>()
                                        .WithTokenProvider(this.TokenProvider);
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            /*
             * HTML CSS JAVASCRIPT: http://jsfiddle.net/vishalvasani/4hqVu/
             * Another one: http://www.uncorkedstudios.com/blog/multipartformdata-file-upload-with-angularjs/
             * 
             * Help: https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/sending-html-form-data-part-2
             * Help3: https://stackoverflow.com/questions/16416601/c-sharp-httpclient-4-5-multipart-form-data-upload?noredirect=1&lq=1
             * help 4: https://stackoverflow.com/questions/7460088/reading-file-input-from-a-multipart-form-data-post
             */
            if (!this.Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(System.Net.HttpStatusCode.UnsupportedMediaType);

            var streamProvider = await this.Request.Content.ReadAsMultipartAsync();
            var content = streamProvider.Contents.First();
            var metadatos = await streamProvider.Contents[1].ReadAsStringAsync();

            var fileName = content.Headers.ContentDisposition.FileName;
            using (var stream = await content.ReadAsStreamAsync())
            {
                var resultado = await this.client.Upload(stream, fileName, metadatos);
                return this.Ok(resultado);
            }
        }

        [HttpPost]
        [Route("eliminar-archivo")]
        public async Task<IHttpActionResult> EliminarArchivo([FromBody]EliminarArchivo cmd)
        {
            await this.client.EliminarArchivo(cmd);
            return this.Ok();
        }

        [HttpPost]
        [Route("restaurar-archivo")]
        public async Task<IHttpActionResult> RestaurarArchivo([FromBody]RestaurarArchivo cmd)
        {
            await this.client.RestaurarArchivo(cmd);
            return this.Ok();
        }
    }
}