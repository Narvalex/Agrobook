using Agrobook.Domain.Common;
using Eventing.Core.Messaging;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ServiciosDenormalizer : SqlDenormalizerV1,
        IHandler<NuevoServicioRegistrado>,
        IHandler<DatosBasicosDelSevicioEditados>,
        IHandler<ServicioEliminado>,
        IHandler<ServicioRestaurado>,
        IHandler<ParcelaDeServicioEspecificada>,
        IHandler<ParcelaDeServicioCambiada>,
        IHandler<PrecioDeServicioFijado>,
        IHandler<PrecioDeServicioAjustado>
    {
        public ServiciosDenormalizer(SqlDenormalizerConfigV1 config)
            : base(config)
        {
        }

        public async Task Handle(long eventNumber, NuevoServicioRegistrado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                context.Servicios.Add(new ServicioEntity
                {
                    Id = e.IdServicio,
                    IdContrato = e.IdContrato,
                    IdOrg = e.IdOrg,
                    IdProd = e.IdProd,
                    Fecha = e.Fecha,
                    Observaciones = e.Observaciones,
                    // Defautls
                    Eliminado = false,
                    IdParcela = null
                });
            });
        }

        public async Task Handle(long eventNumber, DatosBasicosDelSevicioEditados e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);
                servicio.IdContrato = e.IdContrato;
                servicio.IdOrg = e.IdOrg;
                servicio.Fecha = e.Fecha;
                servicio.Observaciones = e.Observaciones;
            });
        }

        public async Task Handle(long eventNumber, ServicioEliminado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);
                servicio.Eliminado = true;
            });
        }

        public async Task Handle(long eventNumber, ServicioRestaurado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);
                servicio.Eliminado = false;
            });
        }

        public async Task Handle(long eventNumber, ParcelaDeServicioEspecificada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.IdParcela = e.IdParcela;
            });
        }

        public async Task Handle(long eventNumber, ParcelaDeServicioCambiada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.IdParcela = e.IdParcela;
            });
        }

        public async Task Handle(long checkpoint, PrecioDeServicioFijado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.TienePrecio = true;
                servicio.PrecioTotal = e.PrecioTotal;
            });
        }

        public async Task Handle(long checkpoint, PrecioDeServicioAjustado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.PrecioTotal = e.PrecioTotal;
            });
        }
    }
}
