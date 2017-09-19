using Agrobook.Common;
using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Common;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ContratosDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevoContrato>,
        IEventHandler<ContratoEditado>,
        IEventHandler<NuevaAdenda>,
        IEventHandler<AdendaEditada>
    {
        public ContratosDenormalizer(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory,
                  typeof(ContratosDenormalizer).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<Contrato>())
        {
        }

        public async Task Handle(long eventNumber, NuevoContrato e)
        {
            await this.Denormalize(eventNumber, context =>
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
            await this.Denormalize(eventNumber, async context =>
            {
                var contrato = await context.Contratos.SingleAsync(x => x.Id == e.IdContrato);
                contrato.Display = e.NombreDelContrato;
                contrato.Fecha = e.Fecha;
            });
        }

        public async Task Handle(long eventNumber, NuevaAdenda e)
        {
            await this.Denormalize(eventNumber, context =>
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
            await this.Denormalize(eventNumber, async context =>
            {
                var adenda = await context.Contratos.SingleAsync(x => x.Id == e.IdAdenda);
                adenda.Display = e.NombreDeLaAdenda;
                adenda.Fecha = e.Fecha;
            });
        }
    }
}
