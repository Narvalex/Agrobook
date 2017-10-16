using Eventing;
using Eventing.Core.Persistence;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static Agrobook.Domain.AgrobookDbContext;

namespace Agrobook.Domain.Common
{
    public class EfCheckpointRepository : ICheckpointRepository
    {
        private readonly Func<AgrobookDbContext> dbContext;
        private readonly Func<AgrobookDbContext> readOnlyDbContext;

        public EfCheckpointRepository(Func<AgrobookDbContext> dbContext, Func<AgrobookDbContext> readOnlyDbContext)
        {
            Ensure.NotNull(dbContext, nameof(dbContext));
            Ensure.NotNull(readOnlyDbContext, nameof(readOnlyDbContext));

            this.dbContext = dbContext;
            this.readOnlyDbContext = readOnlyDbContext;
        }

        public long? GetCheckpoint(string subscriptionId)
        {
            using (var context = this.readOnlyDbContext.Invoke())
            {
                return context.Checkpoints.SingleOrDefault(c => c.Subscription == subscriptionId)?.LastCheckpoint;
            }
        }

        public async Task<long?> GetCheckpointAsync(string subscriptionId)
        {
            using (var context = this.readOnlyDbContext.Invoke())
            {
                return (await context.Checkpoints.SingleOrDefaultAsync(c => c.Subscription == subscriptionId))?.LastCheckpoint;
            }
        }

        public void SaveCheckpoint(string subscriptionId, long checkpoint)
        {
            using (var context = this.dbContext.Invoke())
            {
                var entity = context.Checkpoints.SingleOrDefault(x => x.Subscription == subscriptionId);
                if (entity is null)
                {
                    entity = new CheckpointEntity { Subscription = subscriptionId };
                    context.Checkpoints.Add(entity);
                }

                entity.LastCheckpoint = checkpoint;
                context.SaveChanges();
            }
        }

        public async Task SaveCheckpointAsync(string subscriptionId, long checkpoint)
        {
            using (var context = this.dbContext.Invoke())
            {
                await context.SaveChangesAsync(subscriptionId, checkpoint);
            }
        }
    }
}
