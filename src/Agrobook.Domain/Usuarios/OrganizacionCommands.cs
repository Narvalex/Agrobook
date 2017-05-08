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
}
