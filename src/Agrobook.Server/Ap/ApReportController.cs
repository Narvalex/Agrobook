using Agrobook.Domain.Ap.Services;
using Agrobook.Domain.DataWarehousing.DAOs;
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
        private readonly ApDao dao;

        public ApReportController()
        {
            this.service = ServiceLocator.ResolveSingleton<ApReportQueryService>();
            this.dao = ServiceLocator.ResolveSingleton<ApDao>();
        }

        [HttpGet]
        [Route("lista-de-productores")]
        public async Task<HttpResponseMessage> GetListaDeProductores()
        {
            var viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = @"Reportes\Ap\ListaDeProductores.rdlc";

            var productores = await this.service.ObtenerTodosProductores();

            var reportDataSource = new ReportDataSource("DataSet", productores);
            viewer.LocalReport.DataSources.Add(reportDataSource);
            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);

            var result = this.Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(new MemoryStream(bytes));
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "reporte.pdf";
            return result;
        }

        [HttpGet]
        [Route("planilla-general-de-servicios-de-ap")]
        public async Task<HttpResponseMessage> GetPlanillaGeneralDeServiciosDeAp()
        {
            var viewer = new ReportViewer();
            viewer.LocalReport.ReportPath = @"Reportes\Ap\PlanillaGeneralDeServiciosDeAp.rdlc";

            var dataSet = await this.dao.ObtenerServicios();

            var reportDataSouce = new ReportDataSource("DataSet", dataSet);
            viewer.LocalReport.DataSources.Add(reportDataSouce);
            viewer.LocalReport.Refresh();

            var bytes = viewer.LocalReport.Render("PDF", null, out string mimeType, out string encoding, out string fileNameExtension, out string[] streams, out Warning[] warnings);

            var result = this.Request.CreateResponse(HttpStatusCode.OK);
            result.Content = new StreamContent(new MemoryStream(bytes));
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = "reporte.pdf";
            return result;
        }
    }
}
