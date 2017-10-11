using Eventing.Core.Domain;

namespace Agrobook.Domain.Usuarios
{
    public class OrganizacionSnapshot : Snapshot
    {
        public OrganizacionSnapshot(string streamName, int version, string nombre, string nombreParaMostrar,
            string[] usuarios)
            : base(streamName, version)
        {
            this.Nombre = nombre;
            this.NombreParaMostrar = nombreParaMostrar;
            this.Usuarios = usuarios;
        }

        public string Nombre { get; }
        public string NombreParaMostrar { get; }
        public string[] Usuarios { get; }
    }
}
