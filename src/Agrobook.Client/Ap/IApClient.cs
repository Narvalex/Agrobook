using Agrobook.Domain.Ap.Commands;
using Eventing.Client;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public interface IApClient : ISecuredClient
    {
        Task EliminarAdenda(string idContrato, string idAdenda);
        Task EliminarContrato(string id);
        Task RestaurarAdenda(string idContrato, string idAdenda);
        Task RestaurarContrato(string id);
        Task Send(AjustarPrecioDelServicio cmd);
        Task Send(CambiarParcelaDelServicio cmd);
        Task Send(EditarAdenda cmd);
        Task Send(EditarContrato cmd);
        Task Send(EditarDatosBasicosDelSevicio cmd);
        Task Send(EditarParcela cmd);
        Task Send(EliminarParcela cmd);
        Task Send(EliminarServicio cmd);
        Task Send(EspecificarParcelaDelServicio cmd);
        Task Send(FijarPrecioAlServicio cmd);
        Task<string> Send(RegistrarNuevaAdenda cmd);
        Task<string> Send(RegistrarNuevoContrato cmd);
        Task<string> Send(RegistrarNuevoServicio cmd);
        Task<string> Send(RegistrarParcela cmd);
        Task Send(RestaurarParcela cmd);
        Task Send(RestaurarServicio cmd);
    }
}