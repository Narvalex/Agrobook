using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<OrganizacionEntity> Organizaciones { get; set; }
        public IDbSet<GrupoEntity> Grupos { get; set; }
        public IDbSet<OrganizacionDeUsuarioEntity> OrganizacionesDeUsuarios { get; set; }
        public IDbSet<GrupoDeUsuarioEntity> GruposDeUsuarios { get; set; }

        protected void OnOrganizacionesModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrganizacionEntityMap());
            modelBuilder.Configurations.Add(new GrupoEntityMap());
            modelBuilder.Configurations.Add(new OrganizacionDeUsuarioEntityMap());
            modelBuilder.Configurations.Add(new GrupoDeUsuarioEntityMap());
        }
    }

    public class OrganizacionEntity
    {
        public string OrganizacionId { get; set; }
        public string NombreParaMostrar { get; set; }
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

    public class GrupoEntity
    {
        public string Id { get; set; }
        public string OrganizacionId { get; set; }
        public string Display { get; set; }
    }

    public class GrupoEntityMap : EntityTypeConfiguration<GrupoEntity>
    {
        public GrupoEntityMap()
        {
            this.HasKey(e => new { e.Id, e.OrganizacionId });

            this.ToTable("Grupos");
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

    public class GrupoDeUsuarioEntity
    {
        public string UsuarioId { get; set; }
        public string OrganizacionId { get; set; }
        public string GrupoId { get; set; }
        public string GrupoDisplay { get; set; }
    }

    public class GrupoDeUsuarioEntityMap : EntityTypeConfiguration<GrupoDeUsuarioEntity>
    {
        public GrupoDeUsuarioEntityMap()
        {
            this.HasKey(e => new { e.UsuarioId, e.OrganizacionId, e.GrupoId });

            this.ToTable("GruposDeUsuarios");
        }
    }
}
