using Agrobook.Client.Archivos;
using Eventing.Client.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Archivos
{
    [RoutePrefix("app/archivos/query")]
    public class ArchivosQueryController : ApiFileControllerBase
    {
        private readonly IArchivosQueryClient client;

        public ArchivosQueryController()
        {
            this.client = ServiceLocator.ResolveNewOf<IArchivosQueryClient>().WithTokenProvider(this.TokenProvider);
        }

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

            return PrepareResponse(nombreArchivo, stream);
        }

        [HttpGet]
        [Route("preview/{idColeccion}/{nombreArchivo}/{usuario}")]
        public async Task<HttpResponseMessage> Preview([FromUri]string idColeccion, [FromUri] string nombreArchivo, [FromUri]string usuario) // security bug here
        {
            var stream = await this.client.Preview(idColeccion, nombreArchivo, usuario);

            return PrepareResponse(nombreArchivo, stream);
        }
    }
}