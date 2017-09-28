using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<ServicioEntity> Servicios { get; set; }

        protected void AddServiciosModel(DbModelBuilder builder)
        {
            builder.Configurations.Add(new ServicioEntityMap());
        }
    }

    public class ServicioEntity
    {
        public string Id { get; set; }
        public string IdContrato { get; set; }
        public string IdOrg { get; set; }
        public string IdProd { get; set; }
        public DateTime Fecha { get; set; }

        // Defaults
        public bool Eliminado { get; set; }
        public string IdParcela { get; set; }
    }

    public class ServicioEntityMap : EntityTypeConfiguration<ServicioEntity>
    {
        public ServicioEntityMap()
        {
            this.HasKey(x => x.Id);

            this.ToTable("Servicios");
        }
    }
}
