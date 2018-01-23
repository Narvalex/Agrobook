using Agrobook.Domain;
using Agrobook.Domain.Ap.Services;
using Eventing.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public interface IApQueryClient : ISecuredClient
    {
        Task<ProdDto> GetProd(string idProd);
        Task<ServicioDto> GetServicio(string idServicio);
        Task<IList<ServicioDto>> GetServiciosPorOrg(string idOrg);
        Task<IList<ContratoConServicios>> GetServiciosPorOrgAgrupadosPorContrato(string idOrg);
        Task<IList<ServicioDto>> GetServiciosPorProd(string idProd);
        Task<IList<ServicioParaDashboardDto>> GetUltimosServicios(int cantidad);
        Task<IList<ClienteDeApDto>> ObtenerClientes(string filtro);
        Task<IList<ContratoEntity>> ObtenerContratos(string idOrg);
        Task<OrgDto> ObtenerOrg(string idOrg);
        Task<IList<OrgConContratosDto>> ObtenerOrgsConContratosDelProductor(string idProd);
        Task<ParcelaEntity> ObtenerParcela(string idParcela);
        Task<IList<ParcelaEntity>> ObtenerParcelas(string idProd);
    }
}