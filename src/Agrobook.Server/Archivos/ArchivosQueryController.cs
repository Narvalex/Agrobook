using Agrobook.Domain.Archivos.Services;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Archivos
{
    [RoutePrefix("archivos/query")]
    public class ArchivosQueryController : ApiController
    {
        private readonly ArchivosQueryService service = ServiceLocator.ResolveSingleton<ArchivosQueryService>();

        [HttpGet]
        [Route("productores")]
        public async Task<IHttpActionResult> ObtenerProductores()
        {
            var dto = await this.service.ObtenerProductores();
            return this.Ok(dto);
        }

        [HttpGet]
        [Route("archivos-del-productor/{idProductor}")]
        public async Task<IHttpActionResult> ObtenerArchivosDelProductor([FromUri]string idProductor)
        {
            var dto = await this.service.ObtenerArchivosDelProductor(idProductor);
            return this.Ok(dto);
        }
    }
}
