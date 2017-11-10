using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<ParcelaEntity> Parcelas { get; set; }

        protected void AddProductoresDeApModel(DbModelBuilder builder)
        {
            builder.Configurations.Add(new ParcelaEntityMap());
        }
    }

    public class ParcelaEntity
    {
        public string Id { get; set; }
        public string IdProd { get; set; }
        public string Display { get; set; }
        public decimal Hectareas { get; set; }
        public bool Eliminado { get; set; }
    }

    public class ParcelaEntityMap : EntityTypeConfiguration<ParcelaEntity>
    {
        public ParcelaEntityMap()
        {
            this.HasKey(e => e.Id);

            this.ToTable("Parcelas");
        }
    }
}

