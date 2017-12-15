using Agrobook.Domain.Common;
using Agrobook.Domain.DataWarehousing.Dimensions;
using Agrobook.Domain.DataWarehousing.Facts;
using System.Data.Entity;

namespace Agrobook.Domain.DataWarehousing
{
    public class AgrobookDataWarehouseContext : SubscribedDbContext
    {
        public AgrobookDataWarehouseContext(bool readOnly, string nameOrConnectionString)
            : base(readOnly, nameOrConnectionString)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dimensions
            modelBuilder.Configurations
                .Add(new ContratoDimMap())
                .Add(new OrganizacionDimMap())
                .Add(new ParcelaDimMap())
                .Add(new PrecioPorHaServicioApDimMap())
                .Add(new TiempoDimMap())
                .Add(new UsuarioDimMap())

            // Facts
                .Add(new ServicioDeApFactMap());
        }

        // Dimensions
        public IDbSet<ApContratoDim> ContratoDims { get; set; }
        public IDbSet<OrganizacionDim> OrganizacionDims { get; set; }
        public IDbSet<ParcelaDim> ParcelaDims { get; set; }
        public IDbSet<ApPrecioPorHaServicioDim> PrecioPorHaServicioApDims { get; set; }
        public IDbSet<TiempoDim> TiempoDims { get; set; }
        public IDbSet<UsuarioDim> UsuarioDims { get; set; }
        // Facts
        public IDbSet<ServicioDeApFact> ServicioDeApFacts { get; set; }
    }
}
