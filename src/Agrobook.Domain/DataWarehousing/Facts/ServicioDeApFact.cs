using Agrobook.Domain.DataWarehousing.Dimensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Facts
{
    // Relantionships in EF: https://msdn.microsoft.com/en-us/library/jj591620(v=vs.113).aspx
    public class ServicioDeApFact
    {
        public int Sid { get; set; }

        public string IdServicio { get; set; }

        public virtual TiempoDim Fecha { get; set; }
        public virtual OrganizacionDim Organizacion { get; set; }
        public virtual ApContratoDim Contrato { get; set; }
        public virtual UsuarioDim Productor { get; set; }
        public virtual ParcelaDim Parcela { get; set; }
        public virtual ApPrecioPorHaServicioDim ApPrecioPorHaServicio { get; set; }
        public virtual DepartamentoDim Departamento { get; set; }
        public virtual UsuarioDim UsuarioQueRegistro { get; set; }

        public decimal PrecioTotal { get; set; }
    }

    public class ServicioDeApFactMap : EntityTypeConfiguration<ServicioDeApFact>
    {
        public ServicioDeApFactMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("ServicioDeApFacts");

            this.Property(x => x.IdServicio)
               .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            this.HasOptional(e => e.Fecha);
            this.HasOptional(e => e.Organizacion);
            this.HasOptional(e => e.Contrato);
            this.HasOptional(e => e.Productor);
            this.HasOptional(e => e.Parcela);
            this.HasOptional(e => e.ApPrecioPorHaServicio);
            this.HasOptional(e => e.Departamento);
            this.HasOptional(e => e.UsuarioQueRegistro);
        }
    }
}
