using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Eventing.EntityFramework.Persistence
{
    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(bool readOnly, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (readOnly)
                this.Configuration.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new EventDescriptorMap());
            modelBuilder.Configurations.Add(new SubscriptionCheckpointMap());
        }

        public IDbSet<EventDescriptor> Events { get; set; }
        public IDbSet<SubscriptionCheckpoint> SubscriptionCheckpoints { get; set; }
    }

    public class EventDescriptor
    {
        public long? Position { get; set; }

        public string StreamCategory { get; set; }
        public string StreamId { get; set; }
        public long Version { get; set; }

        public string EventType { get; set; }
        public string Payload { get; set; }

        public Guid CommitId { get; set; }
        public Guid EventId { get; set; }

        public DateTime TimeStamp { get; set; }
    }

    public class EventDescriptorMap : EntityTypeConfiguration<EventDescriptor>
    {
        public EventDescriptorMap()
        {
            this.HasKey(x => new { x.StreamCategory, x.StreamId, x.Version });
            this.Property(x => x.Position).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.HasIndex(x => x.Position);

            this.ToTable("Events");
        }
    }

    public class SubscriptionCheckpoint
    {
        public string SubscriptionId { get; set; }
        public long? Checkpoint { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class SubscriptionCheckpointMap : EntityTypeConfiguration<SubscriptionCheckpoint>
    {
        public SubscriptionCheckpointMap()
        {
            this.HasKey(x => x.SubscriptionId);

            this.ToTable("SubscriptionCheckpoints");
        }
    }
}
