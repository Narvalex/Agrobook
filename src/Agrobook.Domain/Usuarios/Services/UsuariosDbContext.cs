using Agrobook.Domain.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosDbContext : AgrobookDbContext
    {
        public UsuariosDbContext(bool isReadonly, string nameOrConnectionString) : base(isReadonly, nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new UsuariosEntityMap());
        }

        public IDbSet<UsuariosEntity> Usuarios { get; set; }
    }

    public class UsuariosEntity
    {
        public string NombreDeUsuario { get; set; }
        public string NombreParaMostrar { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class UsuariosEntityMap : EntityTypeConfiguration<UsuariosEntity>
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
