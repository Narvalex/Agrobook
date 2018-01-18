using System;

namespace Agrobook.Domain.Common
{
    /// <summary>
    /// This is not being used in new denormalizers
    /// </summary>
    public class AgrobookSqlDenormalizerConfig
    {
        public AgrobookSqlDenormalizerConfig(Func<AgrobookDbContext> contextFactory, string subscriptionId)
        {
            this.ContextFactory = contextFactory;
            this.SubscriptionId = subscriptionId;
        }

        public Func<AgrobookDbContext> ContextFactory { get; }
        public string SubscriptionId { get; }
    }
}
