using Agrobook.Core;
using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoUsuarioCreado : MensajeAuditable, IEvent
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

        public string StreamId => this.Usuario;
    }

    public class UsuarioInicioSesion : MensajeAuditable, IEvent
    {
        public UsuarioInicioSesion(Firma metadatos)
            : base(metadatos)
        { }

        public string StreamId => this.Firma.Usuario;
    }

    public class AvatarUrlActualizado : MensajeAuditable, IEvent
    {
        public AvatarUrlActualizado(Firma metadatos, string usuario, string nuevoAvatarUrl)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.NuevoAvatarUrl = nuevoAvatarUrl;
        }

        public string Usuario { get; }
        public string NuevoAvatarUrl { get; }

        public string StreamId => this.Usuario;
    }

    public class NombreParaMostrarActualizado : MensajeAuditable, IEvent
    {
        public NombreParaMostrarActualizado(Firma metadatos, string usuario, string nuevoNombreParaMostrar)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.NuevoNombreParaMostrar = nuevoNombreParaMostrar;
        }

        public string Usuario { get; }
        public string NuevoNombreParaMostrar { get; }

        public string StreamId => this.Usuario;
    }

    public class PasswordCambiado : MensajeAuditable, IEvent
    {
        public PasswordCambiado(Firma metadatos, string usuario, string loginInfoEncriptado)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.LoginInfoEncriptado = loginInfoEncriptado;
        }

        public string Usuario { get; }
        public string LoginInfoEncriptado { get; }

        public string StreamId => this.Usuario;
    }

    public class PasswordReseteado : MensajeAuditable, IEvent
    {
        public PasswordReseteado(Firma metadatos, string usuario, string loginInfoEncriptado)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.LoginInfoEncriptado = loginInfoEncriptado;
        }

        public string Usuario { get; }
        public string LoginInfoEncriptado { get; }

        public string StreamId => this.Usuario;
    }

    public class PermisoRetiradoDelUsuario : MensajeAuditable, IEvent
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

        public string StreamId => this.IdUsuario;
    }

    public class PermisoOtorgadoAlUsuario : MensajeAuditable, IEvent
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

        public string StreamId => this.IdUsuario;
    }
}
