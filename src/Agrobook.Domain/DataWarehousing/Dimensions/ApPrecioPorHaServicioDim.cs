using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class ApPrecioPorHaServicioDim
    {
        public int Sid { get; set; }
        public decimal Precio { get; set; }
        public string Moneda { get; set; }

        public static ApPrecioPorHaServicioDim GetOrAdd(decimal precioTotal, decimal hectareas, IDbSet<ApPrecioPorHaServicioDim> dbSet)
        {
            var precioPorHa = decimal.Round(precioTotal / hectareas, 2);
            var dim = dbSet.SingleOrDefault(x => x.Precio == precioPorHa);
            if (dim == null)
            {
                dim = new ApPrecioPorHaServicioDim
                {
                    Precio = precioPorHa,
                    Moneda = Ap.ValueObjects.Moneda.DolaresAmericanos
                };

                dbSet.Add(dim);
            }
            return dim;
        }
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
