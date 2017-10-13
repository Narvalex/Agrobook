using Eventing.Core.Domain;

namespace Agrobook.Domain.Usuarios
{
    public class OrganizacionSnapshot : Snapshot
    {
        public OrganizacionSnapshot(string streamName, int version, string nombre, string nombreParaMostrar,
            string[] usuarios, bool estaEliminada)
            : base(streamName, version)
        {
            this.Nombre = nombre;
            this.NombreParaMostrar = nombreParaMostrar;
            this.Usuarios = usuarios;
            this.EstaEliminada = estaEliminada;
        }

        public string Nombre { get; }
        public string NombreParaMostrar { get; }
        public string[] Usuarios { get; }
        public bool EstaEliminada { get; }
    }
}
