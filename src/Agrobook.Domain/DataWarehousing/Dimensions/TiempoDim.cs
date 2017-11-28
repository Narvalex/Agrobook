using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class TiempoDim
    {
        public int Sid { get; set; }
        public int Año { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
    }

    public class TiempoDimMap : EntityTypeConfiguration<TiempoDim>
    {
        public TiempoDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("TiempoDims");
        }
    }
}
