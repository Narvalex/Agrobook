using Eventing;
using System;
using System.Linq;

namespace Agrobook.Domain.Common
{
    public class EfCheckpointProvider<TSubscribedDbContext> where TSubscribedDbContext : SubscribedDbContext
    {
        private readonly Func<TSubscribedDbContext> readOnlyDbContext;

        public EfCheckpointProvider(Func<TSubscribedDbContext> readOnlyDbContext)
        {
            Ensure.NotNull(readOnlyDbContext, nameof(readOnlyDbContext));

            this.readOnlyDbContext = readOnlyDbContext;
        }

        public long? GetCheckpoint()
        {
            using (var context = this.readOnlyDbContext.Invoke())
            {
                return context.Checkpoint.FirstOrDefault()?.LastCheckpoint;
            }
        }
    }
}
