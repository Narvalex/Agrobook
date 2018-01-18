using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<ContratoEntity> Contratos { get; set; }

        protected void AddContratosModel(DbModelBuilder builder)
        {
            builder.Configurations.Add(new ContratoEntityMap());
        }
    }

    public class ContratoEntity
    {
        public string Id { get; set; }
        public string IdOrg { get; set; }
        public string Display { get; set; }
        public bool EsAdenda { get; set; }
        public bool Eliminado { get; set; }
        public bool TieneArchivo { get; set; }
        public string IdContratoDeLaAdenda { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class ContratoEntityMap : EntityTypeConfiguration<ContratoEntity>
    {
        public ContratoEntityMap()
        {
            this.HasKey(e => e.Id);

            this.ToTable("Contratos");
        }
    }
}
