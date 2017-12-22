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
        private readonly IApReportClient client;

        public ApReportController()
        {
            this.client = ServiceLocator.ResolveNewOf<IApReportClient>().WithTokenProvider(this.TokenProvider);
        }

        [HttpGet]
        [Route("lista-de-productores")]
        public async Task<HttpResponseMessage> GetListaDeProductores()
        {
            var reporte = await this.client.GetReporteDeListaDeProductores();
            var response = this.PrepareResponse("ListaDeProductores.pdf", reporte);
            return response;
        }

        [HttpGet]
        [Route("planilla-general-de-servicios-de-ap")]
        public async Task<HttpResponseMessage> GetPlanillaGeneral()
        {
            var reporte = await this.client.GetPlanillaGeneral();
            var response = this.PrepareResponse("PlanillaGeneralDeServiciosDeAp.pdf", reporte);
            return response;
        }
    }
}