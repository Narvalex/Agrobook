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
        IEventHandler<NuevoServicioRegistrado>
    {
        public ServiciosDenormalizer(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory, typeof(ServiciosDenormalizer).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<Servicio>())
        {
        }

        public async Task HandleOnce(long eventNumber, NuevoServicioRegistrado e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var orgDisplay = (await context
                                .Organizaciones
                                .SingleOrDefaultAsync(x => x.OrganizacionId == e.IdOrg))
                                ?.NombreParaMostrar;

                var contratoDisplay = (await context
                                        .Contratos
                                        .SingleOrDefaultAsync(x => x.Id == e.IdContrato))
                                        ?.Display;

                context.Servicios.Add(new ServicioEntity
                {
                    Id = e.IdServicio,
                    IdContrato = e.IdContrato,
                    IdOrg = e.IdOrg,
                    IdProd = e.IdProd,
                    Fecha = e.Fecha,
                    // Defautls
                    Eliminado = false,
                    ParcelaId = null,
                    ParcelaDisplay = null
                });
            });
        }
    }
}
