using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext : DbContext
    {
        public AgrobookDbContext(bool isForReadOnly, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (isForReadOnly)
                this.Configuration.AutoDetectChangesEnabled = false;
        }

        public IDbSet<CheckpointEntity> Checkpoints { get; set; }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new CheckpointEntityMap());

            this.AddUsuariosModel(builder);
            this.AddOrganizacinoesModel(builder);
            this.AddArchivosModel(builder);
            this.AddContratosModel(builder);
            this.AddProductoresDeApModel(builder);
            this.AddServiciosModel(builder);
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

        public int SaveChanges(string subscriptionName, long? lastCheckpoint)
        {
            var checkpointEntity = this.Checkpoints.SingleOrDefault(x => x.Subscription == subscriptionName);
            if (checkpointEntity == null)
            {
                checkpointEntity = new CheckpointEntity { Subscription = subscriptionName };
                this.Checkpoints.Add(checkpointEntity);
            }

            checkpointEntity.LastCheckpoint = lastCheckpoint;

            return base.SaveChanges();
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
