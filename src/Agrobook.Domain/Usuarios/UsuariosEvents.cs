using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoUsuarioCreado : MensajeAuditable
    {
        public NuevoUsuarioCreado(Metadatos metadatos, string usuario, string loginInfo)
            : base(metadatos)
        {
            this.Usuario = usuario;
            this.LoginInfo = loginInfo;
        }

        public string Usuario { get; }
        public string LoginInfo { get; } // Encriptado
    }

    public class UsuarioInicioSesion : MensajeAuditable
    {
        public UsuarioInicioSesion(Metadatos metadatos)
            : base(metadatos)
        { }
    }
}
