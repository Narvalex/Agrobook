using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<OrganizacionEntity> Organizaciones { get; set; }
        public IDbSet<OrganizacionDeUsuarioEntity> OrganizacionesDeUsuarios { get; set; }

        protected void AddOrganizacinoesModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrganizacionEntityMap());
            modelBuilder.Configurations.Add(new OrganizacionDeUsuarioEntityMap());
        }
    }

    public class OrganizacionEntity
    {
        public string OrganizacionId { get; set; }
        public string NombreParaMostrar { get; set; }
        public bool EstaEliminada { get; set; }
    }

    public class OrganizacionEntityMap : EntityTypeConfiguration<OrganizacionEntity>
    {
        public OrganizacionEntityMap()
        {
            this.HasKey(e => e.OrganizacionId);

            this.ToTable("Organizaciones");
        }
    }

    public class OrganizacionDeUsuarioEntity
    {
        public string UsuarioId { get; set; }
        public string OrganizacionId { get; set; }
        public string OrganizacionDisplay { get; set; }
    }

    public class OrganizacionDeUsuarioEntityMap : EntityTypeConfiguration<OrganizacionDeUsuarioEntity>
    {
        public OrganizacionDeUsuarioEntityMap()
        {
            this.HasKey(e => new { e.UsuarioId, e.OrganizacionDisplay });

            this.ToTable("OrganizacionesDeUsuarios");
        }
    }
}
