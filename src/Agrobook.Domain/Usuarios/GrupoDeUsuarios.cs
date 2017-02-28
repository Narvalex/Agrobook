using Agrobook.Core;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Usuarios
{
    public class GrupoDeUsuarios : EventSourced
    {
        private HashSet<string> usuarios = new HashSet<string>();

        public GrupoDeUsuarios()
        {
            this.On<NuevoGrupoCreado>(e => this.StreamName = e.IdGrupo);
            this.On<UsuarioAgregadoAGrupo>(e => this.usuarios.Add(e.IdUsuario));
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (GrupoDeUsuariosSnapshot)snapshot;
            this.usuarios = new HashSet<string>(state.Usuarios);
        }

        protected override ISnapshot TakeSnapshot() =>
            new GrupoDeUsuariosSnapshot(this.StreamName, this.Version, this.usuarios.ToArray());

        public bool YaPerteneceUsuarioAlGrupo(string usuario) => this.usuarios.Contains(usuario);
    }

    public class GrupoDeUsuariosSnapshot : Snapshot
    {
        public GrupoDeUsuariosSnapshot(string streamName, int version,
            string[] usuarios) : base(streamName, version)
        {
            this.Usuarios = usuarios;
        }

        public string[] Usuarios { get; }
    }
}

