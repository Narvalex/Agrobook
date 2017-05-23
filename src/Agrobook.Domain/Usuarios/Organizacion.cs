using Agrobook.Core;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Usuarios
{
    [StreamCategory("agrobook.organizaciones")]
    public class Organizacion : EventSourced
    {
        private IDictionary<string, string> grupos = new Dictionary<string, string>();

        public Organizacion()
        {
            this.On<NuevaOrganizacionCreada>(e =>
            {
                this.StreamName = e.Identificador;
                this.Nombre = e.Identificador;
                this.NombreParaMostrar = e.NombreParaMostrar;
            });
            this.On<NuevoGrupoCreado>(e =>
            {
                this.grupos.Add(e.GrupoId, e.GrupoDisplayName);
            });
        }

        public string Nombre { get; private set; }
        public string NombreParaMostrar { get; private set; }

        public bool YaTieneGrupoConId(string idGrupo) => this.grupos.ContainsKey(idGrupo);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (OrganizacionSnapshot)snapshot;
            this.Nombre = state.Nombre;
            this.NombreParaMostrar = state.NombreParaMostrar;
            foreach (var item in state.Grupos)
                this.grupos.Add(item);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new OrganizacionSnapshot(this.StreamName, this.Version, this.Nombre, this.NombreParaMostrar,
                this.grupos.ToArray());
        }
    }
}
