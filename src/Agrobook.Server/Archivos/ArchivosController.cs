using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using Eventing.Core.Serialization;
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
        public async Task<IHttpActionResult> UploadV2()
        {
            var content = this.Request.Content;

            if (!content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var streamProvider = await content.ReadAsMultipartAsync();
            var fileContent = streamProvider.Contents.First();

            var metadatosSerializados = await streamProvider.Contents[1].ReadAsStringAsync();
            var metadatos = this.serializer.Deserialize<MetadatosDeArchivo>(metadatosSerializados);

            // command being processed here;
            var command = new AgregarArchivoAColeccion(null, metadatos.IdColeccion,
                new ArchivoDescriptor(metadatos.Nombre, metadatos.Extension, metadatos.Fecha, metadatos.Tipo, metadatos.Size),
                fileContent)
                .ConFirma(this.ActionContext);

            var result = await this.service.HandleAsync(command);

            return this.Ok(result);
        }
    }
}
