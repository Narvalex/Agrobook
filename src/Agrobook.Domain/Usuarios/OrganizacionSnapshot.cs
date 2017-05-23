using Agrobook.Core;
using System.Collections.Generic;

namespace Agrobook.Domain.Usuarios
{
    public class OrganizacionSnapshot : Snapshot
    {
        public OrganizacionSnapshot(string streamName, int version, string nombre, string nombreParaMostrar,
            KeyValuePair<string, string>[] grupos) 
            : base(streamName, version)
        {
            this.Nombre = nombre;
            this.NombreParaMostrar = nombreParaMostrar;
            this.Grupos = grupos;
        }

        public string Nombre { get; }
        public string NombreParaMostrar { get; }
        public KeyValuePair<string, string>[] Grupos { get; }
    }
}
 