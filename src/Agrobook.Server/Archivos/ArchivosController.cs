using Agrobook.Core;
using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Archivos
{
    [RoutePrefix("archivos")]
    public class ArchivosController : ApiController
    {
        private readonly IJsonSerializer serializer = ServiceLocator.ResolveSingleton<IJsonSerializer>();
        private readonly ArchivosService service = ServiceLocator.ResolveSingleton<ArchivosService>();

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            var content = this.Request.Content;

            if (!content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var streamProvider = await content.ReadAsMultipartAsync();
            var fileContent = streamProvider.Contents.First();

            var metadatosSerializados = await streamProvider.Contents[1].ReadAsStringAsync();
            var metadatos = this.serializer.Deserialize<MetadatosDelArchivo>(metadatosSerializados);


            var command = new AgregarArchivoAColeccion(null, metadatos.IdProductor,
                new ArchivoDescriptor(metadatos.Nombre,
                            metadatos.Extension,
                            metadatos.Fecha,
                            metadatos.Desc,
                            metadatos.Size),
                fileContent)
            .ConMetadatos(this.ActionContext);

            var dto = await this.service.HandleAsync(command);

            return this.Ok(dto);
        }
    }


}
