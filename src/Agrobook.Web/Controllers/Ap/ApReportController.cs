using Agrobook.Client.Ap;
using Eventing.Client.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Web.Controllers.Ap
{
    [RoutePrefix("app/ap/report")]
    public class ApReportController : ApiFileControllerBase
    {
        private readonly ApReportClient client;

        public ApReportController()
        {
            this.client = ServiceLocator.ResolveNewOf<ApReportClient>().WithTokenProvider(this.TokenProvider);
        }

        [HttpGet]
        [Route("lista-de-productores")]
        public async Task<HttpResponseMessage> GetListaDeProductores()
        {
            var reporte = await this.client.GetReporteDeListaDeProductores();
            var response = this.PrepareResponse("ListaDeProductores.pdf", reporte);
            return response;
        }
    }
}