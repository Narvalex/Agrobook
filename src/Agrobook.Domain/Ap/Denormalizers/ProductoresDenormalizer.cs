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
    public class ProductoresDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevaParcelaRegistrada>,
        IEventHandler<ParcelaEditada>,
        IEventHandler<ParcelaEliminada>,
        IEventHandler<ParcelaRestaurada>
    {
        public ProductoresDenormalizer(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory,
                  typeof(ProductoresDenormalizer).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<Productor>())
        {
        }

        public async Task HandleOnce(long eventNumber, NuevaParcelaRegistrada e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                context.Parcelas.Add(new ParcelaEntity
                {
                    Id = e.IdParcela,
                    Display = e.NombreDeLaParcela,
                    IdProd = e.IdProductor,
                    Hectareas = e.Hectareas.ToString(),
                    Eliminado = false
                });
            });
        }

        public async Task HandleOnce(long eventNumber, ParcelaEditada e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var parcela = await context.Parcelas.SingleAsync(x => x.Id == e.IdParcela);
                parcela.Display = e.Nombre;
                parcela.Hectareas = e.Hectareas.ToString();
            });
        }

        public async Task HandleOnce(long eventNumber, ParcelaEliminada e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var parcela = await context.Parcelas.SingleAsync(x => x.Id == e.IdParcela);
                parcela.Eliminado = true;
            });
        }

        public async Task HandleOnce(long eventNumber, ParcelaRestaurada e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var parcela = await context.Parcelas.SingleAsync(x => x.Id == e.IdParcela);
                parcela.Eliminado = false;
            });
        }
    }
}
