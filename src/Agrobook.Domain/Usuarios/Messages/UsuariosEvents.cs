using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoUsuarioCreado : MensajeAuditable
    {
        public NuevoUsuarioCreado(Firma metadatos, string usuario, string nombreParaMostrar, string avatarUrl, string loginInfoEncriptado)
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
        public UsuarioInicioSesion(Firma metadatos)
            : base(metadatos)
        { }
    }

    public class AvatarUrlActualizado : MensajeAuditable
    {
        public AvatarUrlActualizado(Firma metadatos, string usuario, string nuevoAvatarUrl)
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
        public NombreParaMostrarActualizado(Firma metadatos, string usuario, string nuevoNombreParaMostrar)
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
        public PasswordCambiado(Firma metadatos, string usuario, string loginInfoEncriptado)
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
        public PasswordReseteado(Firma metadatos, string usuario, string loginInfoEncriptado)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.LoginInfoEncriptado = loginInfoEncriptado;
        }

        public string Usuario { get; }
        public string LoginInfoEncriptado { get; }
    }

    public class PermisoRetiradoDelUsuario : MensajeAuditable
    {
        public PermisoRetiradoDelUsuario(Firma metadatos, string idUsuario, string permiso, string loginInfoActualizado) : base(metadatos)
        {
            this.IdUsuario = idUsuario;
            this.Permiso = permiso;
            this.LoginInfoActualizado = loginInfoActualizado;
        }

        public string IdUsuario { get; }
        public string Permiso { get; }
        public string LoginInfoActualizado { get; }
    }

    public class PermisoOtorgadoAlUsuario : MensajeAuditable
    {
        public PermisoOtorgadoAlUsuario(Firma metadatos, string idUsuario, string permiso, string loginInfoActualizado) : base(metadatos)
        {
            this.IdUsuario = idUsuario;
            this.Permiso = permiso;
            this.LoginInfoActualizado = loginInfoActualizado;
        }

        public string IdUsuario { get; }
        public string Permiso { get; }
        public string LoginInfoActualizado { get; }
    }
}
