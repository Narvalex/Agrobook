using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevaOrganizacionCreada : MensajeAuditable
    {
        public NuevaOrganizacionCreada(Firma metadatos, string identificador, string nombreParaMostrar) : base(metadatos)
        {
            this.Identificador = identificador;
            this.NombreParaMostrar = nombreParaMostrar;
        }

        public string Identificador { get; }
        public string NombreParaMostrar { get; }
    }

    public class NuevoGrupoCreado : MensajeAuditable
    {
        public NuevoGrupoCreado(Firma metadatos, string grupoId, string grupoDisplayName, string organizacionId) : base(metadatos)
        {
            this.GrupoId = grupoId;
            this.GrupoDisplayName = grupoDisplayName;
            this.OrganizacionId = organizacionId;
        }

        public string GrupoId { get; }
        public string GrupoDisplayName { get; }
        public string OrganizacionId { get; }
    }

    public class UsuarioAgregadoALaOrganizacion : MensajeAuditable
    {
        public UsuarioAgregadoALaOrganizacion(Firma metadatos, string organizacionId, string usuarioId) : base(metadatos)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
    }

    public class UsuarioAgregadoAUnGrupo : MensajeAuditable
    {
        public UsuarioAgregadoAUnGrupo(Firma metadatos, string organizacionId, string usuarioId, string grupoId) : base(metadatos)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
            this.GrupoId = grupoId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
        public string GrupoId { get; }
    }

    public class UsuarioRemovidoDeUnGrupo : MensajeAuditable
    {
        public UsuarioRemovidoDeUnGrupo(Firma metadatos, string organizacionId, string usuarioId, string grupoId) : base(metadatos)
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
