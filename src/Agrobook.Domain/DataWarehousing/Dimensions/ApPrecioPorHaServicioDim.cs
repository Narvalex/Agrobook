using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class ApPrecioPorHaServicioDim
    {
        public int Sid { get; set; }
        public decimal Precio { get; set; }
        public string Moneda { get; set; }
    }

    public class PrecioPorHaServicioApDimMap : EntityTypeConfiguration<ApPrecioPorHaServicioDim>
    {
        public PrecioPorHaServicioApDimMap()
        {
            this.HasKey(e => e.Sid);

            this.Property(x => x.Precio)
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            this.ToTable("PrecioPorHaServicioApDims");
        }
    }
}
