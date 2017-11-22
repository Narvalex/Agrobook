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

    public class CambiarNombreDeOrganizacion : MensajeAuditable
    {
        public CambiarNombreDeOrganizacion(Firma firma, string idOrg, string nombre) : base(firma)
        {
            this.IdOrg = idOrg;
            this.Nombre = nombre;
        }

        public string IdOrg { get; }
        public string Nombre { get; }
    }

    public class EliminarOrganizacion : MensajeAuditable
    {
        public EliminarOrganizacion(Firma firma, string idOrg) : base(firma)
        {
            this.IdOrg = idOrg;
        }

        public string IdOrg { get; }
    }

    public class RestaurarOrganizacion : MensajeAuditable
    {
        public RestaurarOrganizacion(Firma firma, string idOrg) : base(firma)
        {
            this.IdOrg = idOrg;
        }

        public string IdOrg { get; }
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
