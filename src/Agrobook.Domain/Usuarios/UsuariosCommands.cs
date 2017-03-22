using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Usuarios
{
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
}
