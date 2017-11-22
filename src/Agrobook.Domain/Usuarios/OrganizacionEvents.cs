using Agrobook.Core;
using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevaOrganizacionCreada : MensajeAuditable, IEvent
    {
        public NuevaOrganizacionCreada(Firma firma, string identificador, string nombreParaMostrar) : base(firma)
        {
            this.Identificador = identificador;
            this.NombreParaMostrar = nombreParaMostrar;
        }

        public string Identificador { get; }
        public string NombreParaMostrar { get; }

        public string StreamId => this.Identificador;
    }

    public class NombreDeOrganizacionCambiado : MensajeAuditable, IEvent
    {
        public NombreDeOrganizacionCambiado(Firma firma, string idOrg, string nombreAntiguo, string nombreNuevo)
            : base(firma)
        {
            this.IdOrg = idOrg;
            this.NombreAntiguo = nombreAntiguo;
            this.NombreNuevo = nombreNuevo;
        }

        public string IdOrg { get; }
        public string NombreAntiguo { get; }
        public string NombreNuevo { get; }

        public string StreamId => this.IdOrg;
    }

    public class OrganizacionEliminada : MensajeAuditable, IEvent
    {
        public OrganizacionEliminada(Firma firma, string id) : base(firma)
        {
            this.Id = id;
        }

        public string Id { get; }

        public string StreamId => this.Id;
    }

    public class OrganizacionRestaurada : MensajeAuditable, IEvent
    {
        public OrganizacionRestaurada(Firma firma, string id) : base(firma)
        {
            this.Id = id;
        }

        public string Id { get; }

        public string StreamId => this.Id;
    }

    public class UsuarioAgregadoALaOrganizacion : MensajeAuditable, IEvent
    {
        public UsuarioAgregadoALaOrganizacion(Firma firma, string organizacionId, string usuarioId) : base(firma)
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
