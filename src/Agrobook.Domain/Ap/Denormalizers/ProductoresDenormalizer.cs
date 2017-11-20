using Agrobook.Domain.Common;
using Agrobook.Domain.Common.Services;
using Eventing.Core.Messaging;
using System.Linq;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ProductoresDenormalizer : SqlDenormalizer,
        IHandler<NuevaParcelaRegistrada>,
        IHandler<ParcelaEditada>,
        IHandler<ParcelaEliminada>,
        IHandler<ParcelaRestaurada>
    {
        public ProductoresDenormalizer(SqlDenormalizerConfig config)
            : base(config)
        {
        }

        public void Handle(long eventNumber, NuevaParcelaRegistrada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var depto = DepartamentosDelParaguayProvider.GetDepartamentos().Single(x => x.Id == e.Ubicacion.IdDepartamento);
                var distrito = depto.Distritos.Single(x => x.Id == e.Ubicacion.IdDistrito);
                context.Parcelas.Add(new ParcelaEntity
                {
                    Id = e.IdParcela,
                    Display = e.NombreDeLaParcela,
                    IdProd = e.IdProductor,
                    Hectareas = e.Hectareas,
                    IdDepartamento = depto.Id,
                    DepartamentoDisplay = depto.Display,
                    IdDistrito = distrito.Id,
                    DistritoDisplay = distrito.Display,
                    Eliminado = false
                });
            });
        }

        public void Handle(long eventNumber, ParcelaEditada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var depto = DepartamentosDelParaguayProvider.GetDepartamentos().Single(x => x.Id == e.Ubicacion.IdDepartamento);
                var distrito = depto.Distritos.Single(x => x.Id == e.Ubicacion.IdDistrito);

                var parcela = context.Parcelas.Single(x => x.Id == e.IdParcela);
                parcela.Display = e.Nombre;
                parcela.Hectareas = e.Hectareas;
                parcela.IdDepartamento = depto.Id;
                parcela.DepartamentoDisplay = depto.Display;
                parcela.IdDistrito = distrito.Id;
                parcela.DistritoDisplay = distrito.Display;
            });
        }

        public void Handle(long eventNumber, ParcelaEliminada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var parcela = context.Parcelas.Single(x => x.Id == e.IdParcela);
                parcela.Eliminado = true;
            });
        }

        public void Handle(long eventNumber, ParcelaRestaurada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var parcela = context.Parcelas.Single(x => x.Id == e.IdParcela);
                parcela.Eliminado = false;
            });
        }
    }
}
