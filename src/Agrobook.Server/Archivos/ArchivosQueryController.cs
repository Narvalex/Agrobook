using Agrobook.Domain.Archivos.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public async Task<IHttpActionResult>  ObtenerProductores()
        {
            var dto = await this.service.ObtenerProductores();
            return this.Ok(dto);
        }
    }
}
