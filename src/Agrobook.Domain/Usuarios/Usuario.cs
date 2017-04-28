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
                this.AvatarUrl = e.AvatarUrl;
            });
            this.On<AvatarUrlActualizado>(e =>
            {
                this.AvatarUrl = e.NuevoAvatarUrl;
            });
            this.On<NombreParaMostrarActualizado>(e =>
            {
                this.NombreParaMostrar = e.NuevoNombreParaMostrar;
            });
            this.On<PasswordCambiado>(e =>
            {
                this.LoginInfoEncriptado = e.LoginInfoEncriptado;
            });
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (UsuarioSnapshot)snapshot;
            this.NombreParaMostrar = state.NombreParaMostrar;
            this.LoginInfoEncriptado = state.LoginInfoEncriptado;
            this.AvatarUrl = state.AvatarUrl;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new UsuarioSnapshot(this.StreamName, this.Version, this.NombreParaMostrar, this.LoginInfoEncriptado, this.AvatarUrl);
        }

        public string LoginInfoEncriptado { get; private set; }
        public string NombreParaMostrar { get; private set; }
        public string AvatarUrl { get; private set; }
    }

    public class UsuarioSnapshot : Snapshot
    {
        public UsuarioSnapshot(string streamName, int version, string nombreParaMostrar, string loginInfoEncriptado, string avatarUrl) : base(streamName, version)
        {
            this.NombreParaMostrar = nombreParaMostrar;
            this.LoginInfoEncriptado = loginInfoEncriptado;
            this.AvatarUrl = avatarUrl;
        }

        public string NombreParaMostrar { get; }
        public string LoginInfoEncriptado { get; }
        public string AvatarUrl { get; }
    }
}
