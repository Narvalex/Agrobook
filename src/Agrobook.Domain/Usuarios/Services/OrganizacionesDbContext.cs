using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<OrganizacionEntity> Organizaciones { get; set; }
        public IDbSet<GrupoEntity> Grupos { get; set; }

        protected void OnOrganizacionesModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrganizacionEntityMap());
            modelBuilder.Configurations.Add(new GrupoEntityMap());
        }
    }

    public class OrganizacionEntity
    {
        public string OrganizacionId { get; set; }
        public string NombreParaMostrar { get; set; }
    }

    public class GrupoEntity
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string OrganizacionId { get; set; }
    }

    public class OrganizacionEntityMap : EntityTypeConfiguration<OrganizacionEntity>
    {
        public OrganizacionEntityMap()
        {
            this.HasKey(e => e.OrganizacionId);

            this.ToTable("Organizaciones");
            this.Property(e => e.OrganizacionId).HasColumnName("OrganizacionId");
            this.Property(e => e.NombreParaMostrar).HasColumnName("NombreParaMostrar");
        }
    }

    public class GrupoEntityMap : EntityTypeConfiguration<GrupoEntity>
    {
        public GrupoEntityMap()
        {
            this.HasKey(e => e.Id);

            this.ToTable("Grupos");
        }
    }
}
