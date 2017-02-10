using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class NuevoUsuarioCreado : MensajeAuditable
    {
        public NuevoUsuarioCreado(Metadatos metadatos, string usuario)
            : base(metadatos)
        {
            this.Usuario = usuario;
        }

        public string Usuario { get; }
    }
}
