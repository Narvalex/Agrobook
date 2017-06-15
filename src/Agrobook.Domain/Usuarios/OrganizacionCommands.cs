using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class CrearNuevaOrganizacion : MensajeAuditable
    {
        public CrearNuevaOrganizacion(Metadatos metadatos, string nombreCrudo) : base(metadatos)
        {
            this.NombreCrudo = nombreCrudo;
        }

        public string NombreCrudo { get; }
    }

    public class CrearNuevoGrupo : MensajeAuditable
    {
        public CrearNuevoGrupo(Metadatos metadatos, string idOrganizacion, string grupoDisplayName) : base(metadatos)
        {
            this.IdOrganizacion = idOrganizacion;
            this.GrupoDisplayName = grupoDisplayName;
        }

        public string IdOrganizacion { get; }
        public string GrupoDisplayName { get; }
    }

    public class AgregarUsuarioALaOrganizacion : MensajeAuditable
    {
        public AgregarUsuarioALaOrganizacion(Metadatos metadatos, string organizacionId, string usuarioId) : base(metadatos)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
    }

    public class AgregarUsuarioAUnGrupo : MensajeAuditable
    {
        public AgregarUsuarioAUnGrupo(Metadatos metadatos, string organizacionId, string usuarioId, string grupoId) : base(metadatos)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
            this.GrupoId = grupoId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
        public string GrupoId { get; }
    }
}
