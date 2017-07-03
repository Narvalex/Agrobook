using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Threading.Tasks;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext : DbContext
    {
        public AgrobookDbContext(bool isReadonly, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (isReadonly)
                this.Configuration.AutoDetectChangesEnabled = false;
        }

        public IDbSet<CheckpointEntity> Checkpoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CheckpointEntityMap());

            this.OnUsuariosModelCreating(modelBuilder);
            this.OnOrganizacionesModelCreating(modelBuilder);
            this.OnArchivosModelCreating(modelBuilder);
        }

        public async Task<int> SaveChangesAsync(string subscriptionName, long? lastCheckpoint)
        {
            var checkpointEntity = await this.Checkpoints.SingleOrDefaultAsync(x => x.Subscription == subscriptionName);
            if (checkpointEntity == null)
            {
                checkpointEntity = new CheckpointEntity { Subscription = subscriptionName };
                this.Checkpoints.Add(checkpointEntity);
            }

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
