using Agrobook.Domain.DataWarehousing.Dimensions;
using Agrobook.Domain.DataWarehousing.Facts;
using System;
using System.Data.Entity;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing
{
    public partial class AgrobookDataWarehouseContext : DbContext
    {
        public AgrobookDataWarehouseContext(bool readOnly, string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            if (readOnly)
                this.Configuration.AutoDetectChangesEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dimensions
            modelBuilder.Configurations
                .Add(new ContratoDimMap())
                .Add(new DepartamentoDimMap())
                .Add(new ParcelaDimMap())
                .Add(new PrecioPorHaServicioApDimMap())
                .Add(new ProductorDimMap())
                .Add(new TiempoDimMap())

            // Facts
                .Add(new ServicioDeApFactMap());
        }

        public IDbSet<CheckpointEntity> Checkpoint { get; set; }
        // Dimensions
        public IDbSet<ContratoDim> ContratoDims { get; set; }
        public IDbSet<DepartamentoDim> DepartamentoDims { get; set; }
        public IDbSet<ParcelaDim> ParcelaDims { get; set; }
        public IDbSet<PrecioPorHaServicioApDim> PrecioPorHaServicioApDims { get; set; }
        public IDbSet<ProductorDim> ProductorDims { get; set; }
        public IDbSet<TiempoDim> TiempoDims { get; set; }
        // Facts
        public IDbSet<ServicioDeApFact> ServicioDeApFact { get; set; }

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
}
