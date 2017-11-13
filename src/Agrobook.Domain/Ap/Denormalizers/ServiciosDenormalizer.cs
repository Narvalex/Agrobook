using Agrobook.Domain.Ap.Commands;
using Agrobook.Domain.Common;
using Eventing.Core.Messaging;
using System.Linq;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ServiciosDenormalizer : SqlDenormalizer,
        IHandler<NuevoServicioRegistrado>,
        IHandler<DatosBasicosDelSevicioEditados>,
        IHandler<ServicioEliminado>,
        IHandler<ServicioRestaurado>,
        IHandler<ParcelaDeServicioEspecificada>,
        IHandler<ParcelaDeServicioCambiada>,
        IHandler<PrecioDeServicioFijado>,
        IHandler<PrecioDeServicioAjustado>
    {
        public ServiciosDenormalizer(SqlDenormalizerConfig config)
            : base(config)
        {
        }

        public void Handle(long eventNumber, NuevoServicioRegistrado e)
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
                    // Defautls
                    Eliminado = false,
                    IdParcela = null
                });
            });
        }

        public void Handle(long eventNumber, DatosBasicosDelSevicioEditados e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);
                servicio.IdContrato = e.IdContrato;
                servicio.IdOrg = e.IdOrg;
                servicio.Fecha = e.Fecha;
            });
        }

        public void Handle(long eventNumber, ServicioEliminado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);
                servicio.Eliminado = true;
            });
        }

        public void Handle(long eventNumber, ServicioRestaurado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);
                servicio.Eliminado = false;
            });
        }

        public void Handle(long eventNumber, ParcelaDeServicioEspecificada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.IdParcela = e.IdParcela;
            });
        }

        public void Handle(long eventNumber, ParcelaDeServicioCambiada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.IdParcela = e.IdParcela;
            });
        }

        public void Handle(long checkpoint, PrecioDeServicioFijado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.TienePrecio = true;
                servicio.PrecioTotal = e.Precio;
            });
        }

        public void Handle(long checkpoint, PrecioDeServicioAjustado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.Servicios.Single(x => x.Id == e.IdServicio);

                servicio.PrecioTotal = e.Precio;
            });
        }
    }
}
