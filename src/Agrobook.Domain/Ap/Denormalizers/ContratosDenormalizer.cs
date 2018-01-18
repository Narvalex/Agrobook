using Agrobook.Domain.Common;
using Eventing.Core.Messaging;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ContratosDenormalizer : AgrobookSqlDenormalizer,
        IHandler<NuevoContrato>,
        IHandler<ContratoEditado>,
        IHandler<ContratoEliminado>,
        IHandler<ContratoRestaurado>,
        IHandler<NuevaAdenda>,
        IHandler<AdendaEditada>,
        IHandler<AdendaEliminada>,
        IHandler<AdendaRestaurada>
    {
        public ContratosDenormalizer(AgrobookSqlDenormalizerConfig config)
            : base(config)
        {
        }

        public async Task Handle(long eventNumber, NuevoContrato e)
        {
            this.Denormalize(eventNumber, context =>
            {
                context.Contratos.Add(new ContratoEntity
                {
                    Id = e.IdContrato,
                    Display = e.NombreDelContrato,
                    Eliminado = false,
                    EsAdenda = false,
                    Fecha = e.Fecha,
                    IdContratoDeLaAdenda = null,
                    IdOrg = e.IdOrganizacion
                });
            });
        }

        public async Task Handle(long eventNumber, ContratoEditado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var contrato = context.Contratos.Single(x => x.Id == e.IdContrato);
                contrato.Display = e.NombreDelContrato;
                contrato.Fecha = e.Fecha;
            });
        }

        public async Task Handle(long eventNumber, ContratoEliminado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var contrato = context.Contratos.Single(x => x.Id == e.IdContrato);
                contrato.Eliminado = true;
            });
        }

        public async Task Handle(long eventNumber, ContratoRestaurado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var contrato = context.Contratos.Single(x => x.Id == e.IdContrato);
                contrato.Eliminado = false;
            });
        }

        public async Task Handle(long eventNumber, NuevaAdenda e)
        {
            this.Denormalize(eventNumber, context =>
            {
                context.Contratos.Add(new ContratoEntity
                {
                    Id = e.IdAdenda,
                    Display = e.NombreDeLaAdenda,
                    Fecha = e.Fecha,
                    Eliminado = false,
                    EsAdenda = true,
                    IdContratoDeLaAdenda = e.IdContrato,
                    IdOrg = e.IdOrganizacion
                });
            });
        }

        public async Task Handle(long eventNumber, AdendaEditada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var adenda = context.Contratos.Single(x => x.Id == e.IdAdenda);
                adenda.Display = e.NombreDeLaAdenda;
                adenda.Fecha = e.Fecha;
            });
        }

        public async Task Handle(long eventNumber, AdendaEliminada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var adenda = context.Contratos.Single(x => x.Id == e.IdAdenda);
                adenda.Eliminado = true;
            });
        }

        public async Task Handle(long eventNumber, AdendaRestaurada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var adenda = context.Contratos.Single(x => x.Id == e.IdAdenda);
                adenda.Eliminado = false;
            });
        }
    }
}
