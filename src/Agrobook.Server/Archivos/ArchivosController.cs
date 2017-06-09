using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Archivos
{
    [RoutePrefix("archivos")]
    public class ArchivosController : ApiController
    {
        [HttpPost]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            return await Task.Run(() => this.BadRequest());
        }
    }
}
