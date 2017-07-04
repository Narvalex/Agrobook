using Agrobook.Client;
using Agrobook.Client.Archivos;
using System.Threading.Tasks;
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

        [HttpGet]
        [Route("productores")]
        public async Task<IHttpActionResult> ObtenerProductores()
        {
            var dto = await this.client.ObtenerProductores();
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("archivos-del-productor/{idProductor}")]
        public async Task<IHttpActionResult> ObtenerArchivosDelProductor([FromUri]string idProductor)
        {
            var dto = await this.client.ObtenerArchivosDelProductor(idProductor);
            return this.Ok(dto);
        }
    }
}