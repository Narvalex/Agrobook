using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class DepartamentoDim
    {
        public int Sid { get; set; }
        public string IdDepartamento { get; set; }
        public string Nombre { get; set; }
    }

    public class DepartamentoDimMap : EntityTypeConfiguration<DepartamentoDim>
    {
        public DepartamentoDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("DepartamentoDims");
        }
    }
}
