using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public class AgrobookDbContext : DbContext
    {
        public AgrobookDbContext(bool isReadonly, string nameOrConnectionString) 
            : base(nameOrConnectionString)
        {
            if (isReadonly)
                this.Configuration.AutoDetectChangesEnabled = false;
        }

        public IDbSet<CheckpointEntity> Checkpoints { get; set; }

        public async Task<int> SaveChangesAsync(string subscriptionName, long? lastCheckpoint)
        {
            var checkpointEntity = await this.Checkpoints.SingleOrDefaultAsync(x => x.Subscription == subscriptionName);
            if (checkpointEntity == null)
                checkpointEntity = new CheckpointEntity { Subscription = subscriptionName };

            checkpointEntity.LastCheckpoint = lastCheckpoint;

            return await base.SaveChangesAsync();
        }

        public class CheckpointEntity
        {
            public string Subscription { get; set; }
            public long? LastCheckpoint { get; set; }
        }

        public class CheckpointEntityMap : EntityTypeConfiguration<CheckpointEntity>
        {
            public CheckpointEntityMap()
            {
                this.HasKey(e => e.Subscription);

                this.ToTable("Checkpoints");
                this.Property(e => e.Subscription).HasColumnName("Subscription");
                this.Property(e => e.LastCheckpoint).HasColumnName("LastCheckpoint");
            }
        }
    }
}
