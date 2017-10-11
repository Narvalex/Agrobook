using Agrobook.Core;
using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevaOrganizacionCreada : MensajeAuditable, IEvent
    {
        public NuevaOrganizacionCreada(Firma metadatos, string identificador, string nombreParaMostrar) : base(metadatos)
        {
            this.Identificador = identificador;
            this.NombreParaMostrar = nombreParaMostrar;
        }

        public string Identificador { get; }
        public string NombreParaMostrar { get; }

        public string StreamId => this.Identificador;
    }

    public class UsuarioAgregadoALaOrganizacion : MensajeAuditable, IEvent
    {
        public UsuarioAgregadoALaOrganizacion(Firma metadatos, string organizacionId, string usuarioId) : base(metadatos)
        {
            this.OrganizacionId = organizacionId;
            this.UsuarioId = usuarioId;
        }

        public string OrganizacionId { get; }
        public string UsuarioId { get; }

        public string StreamId => this.OrganizacionId;
    }

    public class UsuarioRemovidoDeLaOrganizacion : MensajeAuditable, IEvent
    {
        public UsuarioRemovidoDeLaOrganizacion(Firma firma, string idUsuario, string idOrganizacion) : base(firma)
        {
            this.IdUsuario = idUsuario;
            this.IdOrganizacion = idOrganizacion;
        }

        public string IdUsuario { get; }
        public string IdOrganizacion { get; }

        public string StreamId => this.IdOrganizacion;
    }
}
