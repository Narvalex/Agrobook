using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class CrearNuevoUsuario : MensajeAuditable
    {
        public CrearNuevoUsuario(Metadatos metadatos, string usuario) : base(metadatos)
        {
            this.Usuario = usuario;
        }

        public string Usuario { get; }
    }
}
