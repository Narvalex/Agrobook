using Eventing;
using Eventing.Core.Messaging;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public abstract class EfDenormalizer<TDbContext> where TDbContext : SubscribedDbContext
    {
        private readonly Func<TDbContext> contextFactory;

        public EfDenormalizer(Func<TDbContext> contextFactory)
        {
            Ensure.NotNull(contextFactory, nameof(contextFactory));

            this.contextFactory = contextFactory;
        }

        public void Denormalize(long checkpoint, Action<TDbContext> doDenormalize)
        {
            using (var context = this.contextFactory.Invoke())
            {
                doDenormalize.Invoke(context);
                context.SaveChanges(checkpoint);
            }
        }
    }

    public class NoOpDenormalizer<TDbContext> : EfDenormalizer<TDbContext>, IHandler<object> where TDbContext : SubscribedDbContext
    {
        public NoOpDenormalizer(Func<TDbContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task Handle(long checkpoint, object e) => this.Denormalize(checkpoint, c => { });
    }
}
