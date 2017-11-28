using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class PrecioPorHaServicioApDim
    {
        public int Sid { get; set; }
        public decimal Precio { get; set; }
        public string Moneda { get; set; }
    }

    public class PrecioPorHaServicioApDimMap : EntityTypeConfiguration<PrecioPorHaServicioApDim>
    {
        public PrecioPorHaServicioApDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("PrecioPorHaServicioApDims");
        }
    }
}
