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
}
