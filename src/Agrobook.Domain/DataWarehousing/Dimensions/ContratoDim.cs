using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class ContratoDim
    {
        public int Sid { get; set; }
        public string IdContrato { get; set; }
        public string NombreContrato { get; set; }
    }

    public class ContratoDimMap : EntityTypeConfiguration<ContratoDim>
    {
        public ContratoDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("ContratoDims");
        }
    }
}
