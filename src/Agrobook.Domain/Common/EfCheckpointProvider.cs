using Eventing;
using System;
using System.Linq;

namespace Agrobook.Domain.Common
{
    public class EfCheckpointProvider
    {
        private readonly Func<AgrobookDbContext> readOnlyDbContext;

        public EfCheckpointProvider(Func<AgrobookDbContext> readOnlyDbContext)
        {
            Ensure.NotNull(readOnlyDbContext, nameof(readOnlyDbContext));

            this.readOnlyDbContext = readOnlyDbContext;
        }

        public long? GetCheckpoint(string subscriptionId)
        {
            using (var context = this.readOnlyDbContext.Invoke())
            {
                return context.Checkpoints.SingleOrDefault(c => c.Subscription == subscriptionId)?.LastCheckpoint;
            }
        }
    }
}
