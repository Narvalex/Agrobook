using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoUsuarioCreado : MensajeAuditable
    {
        public NuevoUsuarioCreado(Metadatos metadatos, string usuario, string password)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.Password = password;
        }

        public string Usuario { get; }
        public string Password { get; }
    }

    public class UsuarioInicioSesion : MensajeAuditable
    {
        public UsuarioInicioSesion(Metadatos metadatos)
            : base(metadatos)
        { }
    }
}
