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
        [HttpGet]
        [Route("upload")]
        public async Task<IHttpActionResult> Upload()
        {
            throw new System.NotImplementedException();
        }
    }
}
