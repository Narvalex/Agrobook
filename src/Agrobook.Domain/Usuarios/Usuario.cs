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
                this.LoginInfo = e.LoginInfo;
            });
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (UsuarioSnapshot)snapshot;
            this.LoginInfo = state.LoginInfo;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new UsuarioSnapshot(this.StreamName, this.Version, this.LoginInfo);
        }

        public string LoginInfo { get; private set; }
    }

    public class UsuarioSnapshot : Snapshot
    {
        public UsuarioSnapshot(string streamName, int version, string loginInfo) : base(streamName, version)
        {
            this.LoginInfo = loginInfo;
        }

        public string LoginInfo { get; }
    }
}
