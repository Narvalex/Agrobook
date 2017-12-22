using Eventing.Client;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public interface IApReportClient : ISecuredClient
    {
        Task<Stream> GetPlanillaGeneral();
        Task<Stream> GetReporteDeListaDeProductores();
    }
}