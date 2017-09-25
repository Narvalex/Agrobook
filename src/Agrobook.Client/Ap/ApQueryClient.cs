using Agrobook.Domain;
using Agrobook.Domain.Ap.Services;
using Eventing.Client.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agrobook.Client.Ap
{
    public class ApQueryClient : ClientBase
    {
        public ApQueryClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "ap/query")
        { }

        public async Task<IList<ClienteDeApDto>> ObtenerClientes(string filtro)
            => await base.Get<IList<ClienteDeApDto>>("clientes?filtro=" + filtro);


        public async Task<OrgDto> ObtenerOrg(string idOrg)
            => await base.Get<OrgDto>("org/" + idOrg);

        public async Task<IList<ContratoEntity>> ObtenerContratos(string idOrg)
            => await base.Get<IList<ContratoEntity>>("contratos/" + idOrg);

        public async Task<IList<ParcelaEntity>> ObtenerParcelas(string idProd)
            => await base.Get<IList<ParcelaEntity>>("parcelas/" + idProd);

        public async Task<ParcelaEntity> ObtenerParcela(string idParcela)
            => await base.Get<ParcelaEntity>("parcela/" + idParcela);

        public async Task<IList<OrgConContratosDto>> ObtenerOrgsConContratosDelProductor(string idProd)
            => await base.Get<IList<OrgConContratosDto>>("orgs-con-contratos-del-productor/" + idProd);

        public async Task<ProdDto> GetProd(string idProd)
            => await base.Get<ProdDto>("prod/" + idProd);

        public async Task<IList<ServicioDto>> GetServiciosPorOrg(string idOrg)
            => await base.Get<IList<ServicioDto>>("servicios-por-org/" + idOrg);

        public async Task<IList<ServicioDto>> GetServiciosPorProd(string idProd)
           => await base.Get<IList<ServicioDto>>("servicios-por-prod/" + idProd);

        public async Task<ServicioDto> GetServicio(string idServicio)
            => await base.Get<ServicioDto>("servicio/" + idServicio);
    }
}
