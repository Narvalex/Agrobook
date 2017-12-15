using Eventing.Client.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public class ApReportClient : ClientBase, IApReportClient
    {
        public ApReportClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "ap/report")
        { }

        public async Task<Stream> GetReporteDeListaDeProductores()
        {
            var stream = await base.Get("lista-de-productores");
            return stream;
        }

        public async Task<Stream> GetPlanillaGeneral()
        {
            var stream = await base.Get("planilla-general-de-servicios-de-ap");
            return stream;
        }
    }
}
