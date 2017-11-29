using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class OrganizacionDim
    {
        public int Sid { get; set; }
        public string IdOrganizacion { get; set; }
        public string Nombre { get; set; }
    }

    public class OrganizacionDimMap : EntityTypeConfiguration<OrganizacionDim>
    {
        public OrganizacionDimMap()
        {
            this.HasKey(e => e.Sid);

            this.Property(x => x.IdOrganizacion)
           .HasColumnAnnotation(IndexAnnotation.AnnotationName, new IndexAnnotation(new IndexAttribute()));

            this.ToTable("OrganizacionDims");
        }
    }
}
