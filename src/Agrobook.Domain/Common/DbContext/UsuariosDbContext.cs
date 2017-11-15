using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<UsuarioEntity> Usuarios { get; set; }

        protected void AddUsuariosModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuariosEntityMap());
        }
    }

    public class UsuarioEntity
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
        public bool EsAdmin { get; set; }
        public bool EsGerente { get; set; }
        public bool EsTecnico { get; set; }
        public bool EsProductor { get; set; }
        public bool EsInvitado { get; set; }
        public bool PuedeAdministrarOrganizaciones { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }

    public class UsuariosEntityMap : EntityTypeConfiguration<UsuarioEntity>
    {
        public UsuariosEntityMap()
        {
            this.HasKey(e => e.Id);

            this.ToTable("Usuarios");
        }
    }
}
