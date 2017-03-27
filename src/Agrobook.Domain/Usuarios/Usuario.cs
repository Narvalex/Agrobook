using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    [StreamCategory("agrobook.usuarios")]
    public class Usuario : EventSourced
    {
        public Usuario()
        {
            this.On<NuevoUsuarioCreado>(e =>
            {
                this.StreamName = e.Usuario;
                this.NombreParaMostrar = e.NombreParaMostrar;
                this.LoginInfoEncriptado = e.LoginInfoEncriptado;
            });
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (UsuarioSnapshot)snapshot;
            this.NombreParaMostrar = state.NombreParaMostrar;
            this.LoginInfoEncriptado = state.LoginInfoEncriptado;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new UsuarioSnapshot(this.StreamName, this.Version, this.NombreParaMostrar, this.LoginInfoEncriptado);
        }

        public string LoginInfoEncriptado { get; private set; }
        public string NombreParaMostrar { get; private set; }
    }

    public class UsuarioSnapshot : Snapshot
    {
        public UsuarioSnapshot(string streamName, int version, string nombreParaMostrar, string loginInfoEncriptado) : base(streamName, version)
        {
            this.NombreParaMostrar = nombreParaMostrar;
            this.LoginInfoEncriptado = loginInfoEncriptado;
        }

        public string NombreParaMostrar { get; }
        public string LoginInfoEncriptado { get; }
    }
}
