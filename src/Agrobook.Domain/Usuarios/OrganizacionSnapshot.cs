using Agrobook.Common;
using Eventing.Core.Domain;
using System.Collections.Generic;

namespace Agrobook.Domain.Usuarios
{
    public class OrganizacionSnapshot : Snapshot
    {
        public OrganizacionSnapshot(string streamName, int version, string nombre, string nombreParaMostrar,
            string[] usuarios,
            KeyValuePair<string, IList<string>>[] grupos)
            : base(streamName, version)
        {
            this.Nombre = nombre;
            this.NombreParaMostrar = nombreParaMostrar;
            this.Usuarios = usuarios;
            this.Grupos = grupos;
        }

        public string Nombre { get; }
        public string NombreParaMostrar { get; }
        public string[] Usuarios { get; }
        public KeyValuePair<string, IList<string>>[] Grupos { get; }
    }
}
