using Agrobook.Common;
using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Common;
using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Denormalizers
{
    public class ContratoDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevoContrato>,
        IEventHandler<NuevaAdenda>
    {
        public ContratoDenormalizer(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory, string subscriptionName, string eventSourceStreamName)
            : base(subscriber, contextFactory, subscriptionName, eventSourceStreamName)
        {
        }

        public async Task Handle(long eventNumber, NuevoContrato e)
        {
            throw new NotImplementedException();
        }

        public async Task Handle(long eventNumber, NuevaAdenda e)
        {
            throw new NotImplementedException();
        }
    }
}
