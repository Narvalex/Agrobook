using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class Usuario : EventSourced
    {
        public Usuario()
        {
            this.On<NuevoUsuarioCreado>(e => this.StreamName = e.Usuario);
        }
    }
}
