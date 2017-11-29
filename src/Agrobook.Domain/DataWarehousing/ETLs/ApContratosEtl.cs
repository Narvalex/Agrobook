using Agrobook.Domain.Ap;
using Agrobook.Domain.Common;
using Agrobook.Domain.DataWarehousing.Dimensions;
using Eventing.Core.Messaging;
using System;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing
{
    public class ApContratosEtl : EfDenormalizer<AgrobookDataWarehouseContext>,
        IHandler<NuevoContrato>,
        IHandler<ContratoEditado>,
        IHandler<NuevaAdenda>,
        IHandler<AdendaEditada>
    {
        public ApContratosEtl(Func<AgrobookDataWarehouseContext> contextFactory) : base(contextFactory)
        {
        }

        public void Handle(long checkpoint, NuevoContrato e)
        {
            this.Denormalize(checkpoint, context =>
            {
                context.ContratoDims.Add(new ApContratoDim
                {
                    IdContrato = e.IdContrato,
                    NombreContrato = e.NombreDelContrato,
                    EsAdenda = false
                });
            });
        }

        public void Handle(long checkpoint, ContratoEditado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var contrato = context.ContratoDims.Single(x => x.IdContrato == e.IdContrato);
                contrato.NombreContrato = e.NombreDelContrato;
            });
        }

        public void Handle(long checkpoint, NuevaAdenda e)
        {
            this.Denormalize(checkpoint, context =>
            {
                context.ContratoDims.Add(new ApContratoDim
                {
                    IdContrato = e.IdAdenda,
                    NombreContrato = e.NombreDeLaAdenda,
                    EsAdenda = true,
                    IdContratoDeLaAdenda = e.IdContrato
                });
            });
        }

        public void Handle(long checkpoint, AdendaEditada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var contrato = context.ContratoDims.Single(x => x.IdContrato == e.IdAdenda);
                contrato.NombreContrato = e.NombreDeLaAdenda;
                contrato.IdContratoDeLaAdenda = e.IdContrato;
            });
        }
    }
}
