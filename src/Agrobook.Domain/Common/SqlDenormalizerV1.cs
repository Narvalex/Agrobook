using Eventing;
using Eventing.Core.Messaging;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public abstract class SqlDenormalizerV1 /* IEventHandler IHandler<object>*/
    {
        private readonly Func<AgrobookDbContext> contextFactory;
        private readonly string subscriptionId;

        public SqlDenormalizerV1(SqlDenormalizerConfigV1 config)
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

    public class NoOpSqlDenormalizer : SqlDenormalizerV1, IHandler<object>
    {
        public NoOpSqlDenormalizer(SqlDenormalizerConfigV1 config) : base(config)
        {
        }

        public void Handle(long checkpoint, object e)
        {
            this.Denormalize(checkpoint, c => { });
        }
    }
}
