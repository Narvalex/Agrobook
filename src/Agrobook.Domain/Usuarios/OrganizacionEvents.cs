using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevaOrganizacionCreada : MensajeAuditable
    {
        public NuevaOrganizacionCreada(Metadatos metadatos, string nombre, string nombreParaMostrar) : base(metadatos)
        {
            this.Nombre = nombre;
            this.NombreParaMostrar = nombreParaMostrar;
        }

        public string Nombre { get; }
        public string NombreParaMostrar { get; }
    }
}
