using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class CrearNuevaOrganizacion : MensajeAuditable
    {
        public CrearNuevaOrganizacion(Firma firma, string nombreCrudo) : base(firma)
        {
            this.NombreCrudo = nombreCrudo;
        }

        public string NombreCrudo { get; }
    }

    public class CrearNuevoGrupo : MensajeAuditable
    {
        public CrearNuevoGrupo(Firma firma, string idOrganizacion, string grupoDisplayName) : base(firma)
        {
            this.IdOrganizacion = idOrganizacion;
            this.GrupoDisplayName = grupoDisplayName;
        }

        public string IdOrganizacion { get; }
        public string GrupoDisplayName { get; }
    }

    public class AgregarUsuarioALaOrganizacion : MensajeAuditable
    {
        public AgregarUsuarioALaOrganizacion(Firma firma, string organizacionId, string usuarioId) : base(firma)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
    }

    public class AgregarUsuarioAUnGrupo : MensajeAuditable
    {
        public AgregarUsuarioAUnGrupo(Firma firma, string organizacionId, string usuarioId, string grupoId) : base(firma)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
            this.GrupoId = grupoId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
        public string GrupoId { get; }
    }

    public class RemoverUsuarioDeUnGrupo : MensajeAuditable
    {
        public RemoverUsuarioDeUnGrupo(Firma firma, string organizacionId, string usuarioId, string grupoId) : base(firma)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
            this.GrupoId = grupoId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }
        public string GrupoId { get; }
    }

    public class RemoverUsuarioDeOrganizacion : MensajeAuditable
    {
        public RemoverUsuarioDeOrganizacion(Firma firma, string idUsuario, string idOrganizacion) : base(firma)
        {
            this.IdUsuario = idUsuario;
            this.IdOrganizacion = idOrganizacion;
        }

        public string IdUsuario { get; }
        public string IdOrganizacion { get; }
    }
}
