using Eventing.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Usuarios
{
    [StreamCategory("agrobook.organizaciones")]
    public class Organizacion : EventSourced
    {
        private List<string> usuarios = new List<string>();

        public Organizacion()
        {
            this.On<NuevaOrganizacionCreada>(e =>
            {
                this.SetStreamNameById(e.Identificador);
                this.Nombre = e.Identificador;
                this.NombreParaMostrar = e.NombreParaMostrar;
            });
            this.On<UsuarioAgregadoALaOrganizacion>(e =>
            {
                this.usuarios.Add(e.UsuarioId);
            });
            this.On<UsuarioRemovidoDeLaOrganizacion>(e =>
            {
                this.usuarios.Remove(e.IdUsuario);
            });
            this.On<OrganizacionEliminada>(e =>
            {
                this.EstaEliminada = true;
            });
            this.On<OrganizacionRestaurada>(e =>
            {
                this.EstaEliminada = false;
            });
        }

        public string Nombre { get; private set; }
        public string NombreParaMostrar { get; private set; }

        public bool EstaEliminada { get; private set; } = false;

        public bool LaOrganizacionNoTieneTodaviaUsuarios => this.usuarios.Count == 0;

        public bool TieneAlUsuarioComoMiembro(string usuarioId) => this.usuarios.Any(x => x == usuarioId);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (OrganizacionSnapshot)snapshot;
            this.Nombre = state.Nombre;
            this.NombreParaMostrar = state.NombreParaMostrar;
            this.usuarios.AddRange(state.Usuarios);
            this.EstaEliminada = state.EstaEliminada;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new OrganizacionSnapshot(this.StreamName, this.Version, this.Nombre, this.NombreParaMostrar,
                this.usuarios.ToArray(), this.EstaEliminada);
        }
    }

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
