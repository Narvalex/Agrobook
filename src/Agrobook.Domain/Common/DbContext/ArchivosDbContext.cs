using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Agrobook.Domain
{
    public partial class AgrobookDbContext
    {
        public IDbSet<ArchivosEntity> Archivos { get; set; }

        protected void AddArchivosModel(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ArchivosEntityMap());
        }
    }

    public class ArchivosEntity
    {
        public string IdColeccion { get; set; }
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public int Size { get; set; }
        public bool Eliminado { get; set; }
    }

    public class ArchivosEntityMap : EntityTypeConfiguration<ArchivosEntity>
    {
        public ArchivosEntityMap()
        {
            this.HasKey(x => new { x.IdColeccion, x.Nombre });

            this.ToTable("Archivos");
        }
    }
}
