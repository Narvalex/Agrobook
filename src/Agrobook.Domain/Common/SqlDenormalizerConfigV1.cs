using System;

namespace Agrobook.Domain.Common
{
    public class SqlDenormalizerConfigV1
    {
        public SqlDenormalizerConfigV1(Func<AgrobookDbContext> contextFactory, string subscriptionId)
        {
            this.ContextFactory = contextFactory;
            this.SubscriptionId = subscriptionId;
        }

        public Func<AgrobookDbContext> ContextFactory { get; }
        public string SubscriptionId { get; }
    }
}
