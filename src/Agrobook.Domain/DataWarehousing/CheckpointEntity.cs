using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing
{
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
