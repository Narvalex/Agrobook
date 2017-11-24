using Agrobook.Domain.Ap.Services;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace Agrobook.Server.Ap
{
    [RoutePrefix("ap/report")]
    public class ApReportController : BaseApiController
    {
        private readonly ApReportQueryService service;

        public ApReportController()
        {
            this.service = ServiceLocator.ResolveSingleton<ApReportQueryService>();
        }

        [HttpGet]
        [Route("lista-de-productores")]
        public async Task<HttpResponseMessage> GetListaDeProductores()
        {
            var viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = @"Reportes\Ap\ListaDeProductores.rdlc";

            var productores = await this.service.ObtenerTodosProductores();

            var reportDataSource = new ReportDataSource("ProductorDataSet", productores);
            viewer.LocalReport.DataSources.Add(reportDataSource);
            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);

            var result = this.Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(new MemoryStream(bytes));
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "listaDeProductores.pdf";
            return result;
        }
    }
}
