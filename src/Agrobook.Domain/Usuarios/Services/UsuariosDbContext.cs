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
        public string NombreDeUsuario { get; set; }
        public string NombreParaMostrar { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class UsuariosEntityMap : EntityTypeConfiguration<UsuarioEntity>
    {
        public UsuariosEntityMap()
        {
            this.HasKey(e => e.NombreDeUsuario);

            this.ToTable("Usuarios");
            this.Property(e => e.NombreDeUsuario).HasColumnName("NombreDeUsuario");
            this.Property(e => e.NombreParaMostrar).HasColumnName("NombreCompleto");
        }
    }
}
