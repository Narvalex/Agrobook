using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class ProductorDim
    {
        public int Sid { get; set; }
        public string IdUsuario { get; set; }
        public string NombreProductor { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }

    public class ProductorDimMap : EntityTypeConfiguration<ProductorDim>
    {
        public ProductorDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("ProductorDims");
        }
    }
}
