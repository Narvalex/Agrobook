using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Archivos
{
    [RoutePrefix("archivos")]
    public class ArchivosController : ApiController
    {
        private readonly ArchivosService service = ServiceLocator.ResolveSingleton<ArchivosService>();

        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            Metadatos metadatos;
            try
            {
                metadatos = await this.service.PersistirArchivoDelProductor(this.Request.Content);
            }
            catch (NotMimeMultipartException)
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            catch (ElArchivoYaExisteException)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            var command = new AgregarArchivoAColeccion(null, metadatos.IdProductor,
                                new Archivo(metadatos.Nombre,
                                            metadatos.Extension,
                                            metadatos.Fecha,
                                            metadatos.Desc,
                                            metadatos.Size))
                            .ConMetadatos(this.ActionContext);

            await this.service.HandleAsync(command);

            return this.Ok();
        }
    }


}
