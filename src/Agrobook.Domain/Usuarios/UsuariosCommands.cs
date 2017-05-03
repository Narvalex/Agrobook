using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Usuarios
{
    public class IniciarSesion
    {
        public IniciarSesion(string usuario, string passwordCrudo, DateTime fecha)
        {
            this.Usuario = usuario;
            this.PasswordCrudo = passwordCrudo;
            this.Fecha = fecha;
        }

        public string Usuario { get; }
        public string PasswordCrudo { get; }
        public DateTime Fecha { get; }
    }

    public class CrearNuevoUsuario : MensajeAuditable
    {
        public CrearNuevoUsuario(Metadatos metadatos, string usuario, string nombreParaMostrar, string avatarUrl, string passwordCrudo, string[] claims)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.NombreParaMostrar = nombreParaMostrar;
            this.AvatarUrl = avatarUrl;
            this.PasswordCrudo = passwordCrudo;
            this.Claims = claims;
        }

        public string Usuario { get; }
        public string NombreParaMostrar { get; }
        public string AvatarUrl { get; }
        public string PasswordCrudo { get; }
        public string[] Claims { get; }
    }

    public class ActualizarPerfil : MensajeAuditable
    {
        public ActualizarPerfil(
            Metadatos metadatos,
            string usuario,
            string avatarUrl, 
            string nombreParaMostrar,
            string passwordActual,
            string nuevoPassword
            ) 
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.AvatarUrl = avatarUrl;
            this.NombreParaMostrar = nombreParaMostrar;
            this.PasswordActual = passwordActual;
            this.NuevoPassword = nuevoPassword;
        }

        /// <summary>
        /// El usuario a ser actualizado.
        /// </summary>
        public string Usuario { get; }

        /// <summary>
        /// El nuevo avatar url. Si no se cambio, entonces es nulo
        /// </summary>
        public string AvatarUrl { get; }

        /// <summary>
        /// El nuevo nombre para mostrar. Si no hubo cambio, entonces es nulo.
        /// </summary>
        public string NombreParaMostrar { get; }

        /// <summary>
        /// El password actual, para ver si coincide con el verdadero.
        /// </summary>
        public string PasswordActual { get; }

        /// <summary>
        /// Nuevo password propuesto. Si no se quiere cambiar este, entonces es nulo.
        /// </summary>
        public string NuevoPassword { get; }
    }

    public class ResetearPassword : MensajeAuditable
    {
        public ResetearPassword(Metadatos metadatos, string usuario) : base(metadatos)
        {
            this.Usuario = usuario;
        }

        public string Usuario { get; }
    }
}
