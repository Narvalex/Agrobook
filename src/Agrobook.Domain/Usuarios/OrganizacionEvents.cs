using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevaOrganizacionCreada : MensajeAuditable
    {
        public NuevaOrganizacionCreada(Metadatos metadatos, string identificador, string nombreParaMostrar) : base(metadatos)
        {
            this.Identificador = identificador;
            this.NombreParaMostrar = nombreParaMostrar;
        }

        public string Identificador { get; }
        public string NombreParaMostrar { get; }
    }

    public class NuevoGrupoCreado : MensajeAuditable
    {
        public NuevoGrupoCreado(Metadatos metadatos, string grupoId, string grupoDisplayName) : base(metadatos)
        {
            this.GrupoId = grupoId;
            this.GrupoDisplayName = grupoDisplayName;
        }

        public string GrupoId { get; }
        public string GrupoDisplayName { get; }
    }
}
