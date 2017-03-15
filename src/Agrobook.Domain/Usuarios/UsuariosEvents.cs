using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoUsuarioCreado : MensajeAuditable
    {
        public NuevoUsuarioCreado(Metadatos metadatos, string usuario, string nombreParaMostrar, string avatarUrl, string loginInfoEncriptado)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.NombreParaMostrar = nombreParaMostrar;
            this.LoginInfoEncriptado = loginInfoEncriptado;
            this.AvatarUrl = avatarUrl;
        }

        public string Usuario { get; }
        public string NombreParaMostrar { get; }
        public string AvatarUrl { get; }
        public string LoginInfoEncriptado { get; }
    }

    public class UsuarioInicioSesion : MensajeAuditable
    {
        public UsuarioInicioSesion(Metadatos metadatos)
            : base(metadatos)
        { }
    }
}
