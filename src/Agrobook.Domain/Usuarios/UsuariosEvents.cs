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

    public class AvatarUrlActualizado : MensajeAuditable
    {
        public AvatarUrlActualizado(Metadatos metadatos, string usuario, string nuevoAvatarUrl)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.NuevoAvatarUrl = nuevoAvatarUrl;
        }

        public string Usuario { get; }
        public string NuevoAvatarUrl { get; }
    }

    public class NombreParaMostrarActualizado : MensajeAuditable
    {
        public NombreParaMostrarActualizado(Metadatos metadatos, string usuario, string nuevoNombreParaMostrar)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.NuevoNombreParaMostrar = nuevoNombreParaMostrar;
        }

        public string Usuario { get; }
        public string NuevoNombreParaMostrar { get; }
    }

    public class PasswordCambiado : MensajeAuditable
    {
        public PasswordCambiado(Metadatos metadatos, string usuario, string loginInfoEncriptado)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.LoginInfoEncriptado = loginInfoEncriptado;
        }

        public string Usuario { get; }
        public string LoginInfoEncriptado { get; }
    }

    public class PasswordReseteado : MensajeAuditable
    {
        public PasswordReseteado(Metadatos metadatos, string usuario, string loginInfoEncriptado)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.LoginInfoEncriptado = loginInfoEncriptado;
        }

        public string Usuario { get; }
        public string LoginInfoEncriptado { get; }
    }
}
