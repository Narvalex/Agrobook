using Agrobook.Core;
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
            this.On<PasswordReseteado>(e =>
            {
                this.LoginInfoEncriptado = e.LoginInfoEncriptado;
            });
            this.On<PermisoRetiradoDelUsuario>(e =>
            {
                this.LoginInfoEncriptado = e.LoginInfoActualizado;
            });
            this.On<PermisoOtorgadoAlUsuario>(e =>
            {
                this.LoginInfoEncriptado = e.LoginInfoActualizado;
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
}
