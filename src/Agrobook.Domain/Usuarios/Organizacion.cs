using Agrobook.Core;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Usuarios
{
    [StreamCategory("agrobook.organizaciones")]
    public class Organizacion : EventSourced
    {
        private List<string> usuarios = new List<string>();
        private IDictionary<string, IList<string>> usuariosPorGrupo = new Dictionary<string, IList<string>>();

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
                this.usuariosPorGrupo.Add(e.GrupoId, new List<string>());
            });
            this.On<UsuarioAgregadoALaOrganizacion>(e =>
            {
                this.usuarios.Add(e.UsuarioId);
            });
            this.On<UsuarioAgregadoAUnGrupo>(e =>
            {
                this.usuariosPorGrupo[e.GrupoId].Add(e.UsuarioId);
            });
        }

        public string Nombre { get; private set; }
        public string NombreParaMostrar { get; private set; }

        public bool YaTieneGrupoConId(string idGrupo) => this.usuariosPorGrupo.ContainsKey(idGrupo);

        public bool YaTieneAlUsuarioComoMiembro(string usuarioId) => this.usuarios.Any(x => x == usuarioId);

        public bool YaTieneUsuarioDentroDelGrupo(string grupoId, string usuarioId)
        {
            throw new System.NotImplementedException();
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (OrganizacionSnapshot)snapshot;
            this.Nombre = state.Nombre;
            this.NombreParaMostrar = state.NombreParaMostrar;
            this.usuarios.AddRange(state.Usuarios);
            foreach (var item in state.Grupos)
                this.usuariosPorGrupo.Add(item);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new OrganizacionSnapshot(this.StreamName, this.Version, this.Nombre, this.NombreParaMostrar,
                this.usuarios.ToArray(),
                this.usuariosPorGrupo.ToArray());
        }
    }
}
