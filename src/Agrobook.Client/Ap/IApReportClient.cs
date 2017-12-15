using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public interface IApReportClient
    {
        Task<Stream> GetPlanillaGeneral();
        Task<Stream> GetReporteDeListaDeProductores();
    }
}