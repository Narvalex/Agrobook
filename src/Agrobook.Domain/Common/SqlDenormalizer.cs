using Eventing;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public abstract class SqlDenormalizer /* IEventHandler IHandler<object>*/
    {
        private readonly Func<AgrobookDbContext> contextFactory;
        private readonly string subscriptionId;

        public SqlDenormalizer(SqlDenormalizerConfig config)
        {
            Ensure.NotNull(config, nameof(config));

            this.contextFactory = config.ContextFactory;
            this.subscriptionId = config.SubscriptionId;
        }

        public void Denormalize(long checkpoint, Action<AgrobookDbContext> denorm)
        {
            using (var context = this.contextFactory.Invoke())
            {
                denorm.Invoke(context);
                context.SaveChanges(this.subscriptionId, checkpoint);
            }
        }

        public async Task DenormalizeAsync(long checkpoint, Action<AgrobookDbContext> denorm)
        {
            using (var context = this.contextFactory.Invoke())
            {
                denorm.Invoke(context);
                await context.SaveChangesAsync(this.subscriptionId, checkpoint);
            }
        }

        public async Task DenormalizeAsync(long checkpoint, Func<AgrobookDbContext, Task> denormAsync)
        {
            using (var context = this.contextFactory.Invoke())
            {
                await denormAsync.Invoke(context);
                await context.SaveChangesAsync(this.subscriptionId, checkpoint);
            }
        }
    }
}
