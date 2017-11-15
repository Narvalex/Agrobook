using Eventing.Core.Domain;

namespace Agrobook.Domain.Usuarios
{
    [StreamCategory("agrobook.usuarios")]
    public class Usuario : EventSourced
    {
        public Usuario()
        {
            this.On<NuevoUsuarioCreado>(e =>
            {
                this.SetStreamNameById(e.Usuario);
                this.NombreParaMostrar = e.NombreParaMostrar;
                this.LoginInfoEncriptado = e.LoginInfoEncriptado;
                this.AvatarUrl = e.AvatarUrl;
            });
            this.On<AvatarUrlActualizado>(e => this.AvatarUrl = e.NuevoAvatarUrl);
            this.On<NombreParaMostrarActualizado>(e => this.NombreParaMostrar = e.NuevoNombreParaMostrar);
            this.On<PasswordCambiado>(e => this.LoginInfoEncriptado = e.LoginInfoEncriptado);
            this.On<PasswordReseteado>(e => this.LoginInfoEncriptado = e.LoginInfoEncriptado);
            this.On<PermisoRetiradoDelUsuario>(e => this.LoginInfoEncriptado = e.LoginInfoActualizado);
            this.On<PermisoOtorgadoAlUsuario>(e => this.LoginInfoEncriptado = e.LoginInfoActualizado);
            // Telefono
            this.On<TelefonoDeUsuarioRegistrado>(e => this.Telefono = e.Telefono);
            this.On<TelefonoDeUsuarioActualizado>(e => this.Telefono = e.Telefono);
            this.On<TelefonoDeUsuarioEliminado>(e => this.Telefono = null);
            // Email
            this.On<EmailDeUsuarioRegistrado>(e => this.Email = e.Email);
            this.On<EmailDeUsuarioActualizado>(e => this.Email = e.Email);
            this.On<EmailDeUsuarioEliminado>(e => this.Email = null);
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (UsuarioSnapshot)snapshot;
            this.NombreParaMostrar = state.NombreParaMostrar;
            this.LoginInfoEncriptado = state.LoginInfoEncriptado;
            this.AvatarUrl = state.AvatarUrl;
            this.Telefono = state.Telefono;
            this.Email = state.Email;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new UsuarioSnapshot(this.StreamName, this.Version, this.NombreParaMostrar, this.LoginInfoEncriptado, this.AvatarUrl,
                this.Telefono, this.Email);
        }

        public string LoginInfoEncriptado { get; private set; }
        public string NombreParaMostrar { get; private set; }
        public string AvatarUrl { get; private set; }
        public string Telefono { get; private set; } = null;
        public string Email { get; private set; } = null;

        public bool TieneTelefono => this.Telefono != null;
        public bool TieneEmail => this.Email != null;
    }

    public class UsuarioSnapshot : Snapshot
    {
        public UsuarioSnapshot(string streamName, int version, string nombreParaMostrar, string loginInfoEncriptado, string avatarUrl,
            string telefono, string email)
            : base(streamName, version)
        {
            this.NombreParaMostrar = nombreParaMostrar;
            this.LoginInfoEncriptado = loginInfoEncriptado;
            this.AvatarUrl = avatarUrl;
            this.Telefono = telefono;
            this.Email = email;
        }

        public string NombreParaMostrar { get; }
        public string LoginInfoEncriptado { get; }
        public string AvatarUrl { get; }
        public string Telefono { get; }
        public string Email { get; }
    }
}
