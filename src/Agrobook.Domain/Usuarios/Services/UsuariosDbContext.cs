using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<UsuarioEntity> Usuarios { get; set; }

        protected void OnUsuariosModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuariosEntityMap());
        }
    }

    public class UsuarioEntity
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
        public bool EsProductor { get; set; }
    }

    public class UsuariosEntityMap : EntityTypeConfiguration<UsuarioEntity>
    {
        public UsuariosEntityMap()
        {
            this.HasKey(e => e.Id);

            this.ToTable("Usuarios");
            this.Property(e => e.Id).HasColumnName("Id");
            this.Property(e => e.Display).HasColumnName("Display");
            this.Property(e => e.AvatarUrl).HasColumnName("AvatarUrl");
            this.Property(e => e.EsProductor).HasColumnName("EsProductor");
        }
    }
}
