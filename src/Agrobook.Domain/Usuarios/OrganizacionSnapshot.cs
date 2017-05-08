using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class OrganizacionSnapshot : Snapshot
    {
        public OrganizacionSnapshot(string streamName, int version, string nombre, string nombreParaMostrar) : base(streamName, version)
        {
            this.Nombre = nombre;
            this.NombreParaMostrar = nombreParaMostrar;
        }

        public string Nombre { get; }
        public string NombreParaMostrar { get; }
    }
}
