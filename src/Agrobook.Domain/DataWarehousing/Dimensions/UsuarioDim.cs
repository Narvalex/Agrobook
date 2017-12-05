using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.DataWarehousing.Dimensions
{
    public class UsuarioDim
    {
        public int Sid { get; set; }
        public string IdUsuario { get; set; }
        public string Nombre { get; set; }
        public bool EsTecnico { get; set; }
        public bool EsGerente { get; set; }
        public bool EsProductor { get; set; }
        public bool EsAdmin { get; set; }
    }

    public class UsuarioDimMap : EntityTypeConfiguration<UsuarioDim>
    {
        public UsuarioDimMap()
        {
            this.HasKey(e => e.Sid);

            this.ToTable("UsuarioDims");
        }
    }
}
