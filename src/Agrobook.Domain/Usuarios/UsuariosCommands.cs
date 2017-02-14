using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Usuarios
{
    public class CrearNuevoUsuario : MensajeAuditable
    {
        public CrearNuevoUsuario(Metadatos metadatos, string usuario, string password)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.Password = password;
        }

        public string Usuario { get; }
        public string Password { get; }
    }

    public class IniciarSesion
    {
        public IniciarSesion(string usuario, string password, DateTime fecha)
        {
            this.Usuario = usuario;
            this.Password = password;
            this.Fecha = fecha;
        }

        public string Usuario { get; }
        public string Password { get; }
        public DateTime Fecha { get; }
    }
}
