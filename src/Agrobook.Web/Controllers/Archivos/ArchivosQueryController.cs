using Agrobook.Client;
using Agrobook.Client.Archivos;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpGet]
        [Route("productores")]
        public async Task<IHttpActionResult> ObtenerProductores()
        {
            var dto = await this.client.ObtenerProductores();
            return this.Ok(dto);
        }
    }
}