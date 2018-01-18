using Eventing;
using Eventing.Core.Messaging;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    /// <summary>
    /// This is not being used in new denormalizers
    /// </summary>
    public abstract class AgrobookSqlDenormalizer /* IEventHandler IHandler<object>*/
    {
        private readonly Func<AgrobookDbContext> contextFactory;
        private readonly string subscriptionId;

        public AgrobookSqlDenormalizer(AgrobookSqlDenormalizerConfig config)
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

    public class NoOpSqlDenormalizerV1 : AgrobookSqlDenormalizer, IHandler<object>
    {
        public NoOpSqlDenormalizerV1(AgrobookSqlDenormalizerConfig config) : base(config)
        {
        }

        public async Task Handle(long checkpoint, object e)
        {
            this.Denormalize(checkpoint, c => { });
        }
    }
}
