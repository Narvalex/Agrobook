using System;

namespace Agrobook.Domain.Common
{
    public class SqlDenormalizerConfig
    {
        public SqlDenormalizerConfig(Func<AgrobookDbContext> contextFactory, string subscriptionId)
        {
            this.ContextFactory = contextFactory;
            this.SubscriptionId = subscriptionId;
        }

        public Func<AgrobookDbContext> ContextFactory { get; }
        public string SubscriptionId { get; }
    }
}
