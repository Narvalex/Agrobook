using Agrobook.Domain.Ap;
using Agrobook.Domain.Common;
using Agrobook.Domain.Common.Services;
using Agrobook.Domain.DataWarehousing.Dimensions;
using Eventing.Core.Messaging;
using System;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing
{
    public class ApProductoresEtl : EfDenormalizer<AgrobookDataWarehouseContext>,
        IHandler<NuevaParcelaRegistrada>,
        IHandler<ParcelaEditada>
    {
        public ApProductoresEtl(Func<AgrobookDataWarehouseContext> contextFactory) : base(contextFactory)
        {
        }

        public void Handle(long checkpoint, NuevaParcelaRegistrada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var depto = DepartamentosDelParaguayProvider.GetDepartamentos().Single(x => x.Id == e.Ubicacion.IdDepartamento);
                var distrito = depto.Distritos.Single(x => x.Id == e.Ubicacion.IdDistrito);
                context.ParcelaDims.Add(new ParcelaDim
                {
                    IdParcela = e.IdParcela,
                    Hectareas = e.Hectareas,
                    Departamento = depto.Display,
                    Distrito = distrito.Display
                });
            });
        }

        public void Handle(long checkpoint, ParcelaEditada e)
        {
            this.Denormalize(checkpoint, context =>
            {

                var depto = DepartamentosDelParaguayProvider.GetDepartamentos().Single(x => x.Id == e.Ubicacion.IdDepartamento);
                var distrito = depto.Distritos.Single(x => x.Id == e.Ubicacion.IdDistrito);

                var parcela = context.ParcelaDims.Single(x => x.IdParcela == e.IdParcela);
                parcela.Hectareas = e.Hectareas;
                parcela.Departamento = depto.Display;
                parcela.Distrito = distrito.Display;
            });
        }
    }
}
