using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Agrobook.Domain.Common
{
    public abstract class SubscribedDbContext : DbContext
    {
        protected SubscribedDbContext(bool readOnly, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (readOnly)
                this.Configuration.AutoDetectChangesEnabled = false;
        }

        public IDbSet<CheckpointEntity> Checkpoint { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new CheckpointEntityMap());
        }

        public int SaveChanges(long? checkpoint)
        {
            var chk = this.Checkpoint.SingleOrDefault();
            if (chk == null)
            {
                chk = new CheckpointEntity();
                this.Checkpoint.Add(chk);
            }

            chk.LastCheckpoint = checkpoint;
            return base.SaveChanges();
        }

        public new int SaveChanges() => throw new InvalidOperationException("This database should only be updated through a subscription!");
    }

    public class CheckpointEntity
    {
        public int Id { get; set; }
        public long? LastCheckpoint { get; set; }
    }

    public class CheckpointEntityMap : EntityTypeConfiguration<CheckpointEntity>
    {
        public CheckpointEntityMap()
        {
            this.HasKey(e => e.Id);

            this.ToTable("Checkpoint");
        }
    }
}
