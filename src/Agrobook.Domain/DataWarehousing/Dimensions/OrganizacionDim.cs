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

            this.ToTable("OrganizacionDims");
        }
    }
}
