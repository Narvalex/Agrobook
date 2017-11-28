using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class ParcelaDim
    {
        public int Sid { get; set; }
        public string IdParcela { get; set; }
        public string IdProductor { get; set; }
        public decimal Hectareas { get; set; }
        public string Departamento { get; set; }
        public string Distrito { get; set; }
    }

    public class ParcelaDimMap : EntityTypeConfiguration<ParcelaDim>
    {
        public ParcelaDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("ParcelaDims");
        }
    }
}
