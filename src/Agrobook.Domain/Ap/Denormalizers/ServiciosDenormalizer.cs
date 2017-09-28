using Agrobook.Common;
using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Ap.ServicioSaga;
using Agrobook.Domain.Common;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ServiciosDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevoServicioRegistrado>,
        IEventHandler<DatosBasicosDelSevicioEditados>,
        IEventHandler<ServicioEliminado>,
        IEventHandler<ServicioRestaurado>,
        IEventHandler<ParcelaDeServicioEspecificada>,
        IEventHandler<ParcelaDeServicioCambiada>
    {
        public ServiciosDenormalizer(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory, typeof(ServiciosDenormalizer).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<Servicio>())
        {
        }

        public async Task HandleOnce(long eventNumber, NuevoServicioRegistrado e)
        {
            await this.Denormalize(eventNumber, context =>
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

        public async Task HandleOnce(long eventNumber, DatosBasicosDelSevicioEditados e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var servicio = await context.Servicios.SingleAsync(x => x.Id == e.IdServicio);
                servicio.IdContrato = e.IdContrato;
                servicio.IdOrg = e.IdOrg;
                servicio.Fecha = e.Fecha;
            });
        }

        public async Task HandleOnce(long eventNumber, ServicioEliminado e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var servicio = await context.Servicios.SingleAsync(x => x.Id == e.IdServicio);
                servicio.Eliminado = true;
            });
        }

        public async Task HandleOnce(long eventNumber, ServicioRestaurado e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var servicio = await context.Servicios.SingleAsync(x => x.Id == e.IdServicio);
                servicio.Eliminado = false;
            });
        }

        public async Task HandleOnce(long eventNumber, ParcelaDeServicioEspecificada e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var servicio = await context.Servicios.SingleAsync(x => x.Id == e.IdServicio);

                servicio.IdParcela = e.IdParcela;
            });
        }

        public async Task HandleOnce(long eventNumber, ParcelaDeServicioCambiada e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var servicio = await context.Servicios.SingleAsync(x => x.Id == e.IdServicio);

                servicio.IdParcela = e.IdParcela;
            });
        }
    }
}
