using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class Usuario : EventSourced
    {
        public Usuario()
        {
            this.On<NuevoUsuarioCreado>(e =>
            {
                this.StreamName = e.Usuario;
                this.PasswordEncriptado = e.Password;
            });
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (UsuarioSnapshot)snapshot;
            this.PasswordEncriptado = state.Password;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new UsuarioSnapshot(this.StreamName, this.Version, this.PasswordEncriptado);
        }

        public string PasswordEncriptado { get; private set; }
    }

    public class UsuarioSnapshot : Snapshot
    {
        public UsuarioSnapshot(string streamName, int version, string password) : base(streamName, version)
        {
            this.Password = password;
        }

        public string Password { get; }
    }
}
