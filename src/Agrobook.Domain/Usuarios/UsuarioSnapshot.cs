using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
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
