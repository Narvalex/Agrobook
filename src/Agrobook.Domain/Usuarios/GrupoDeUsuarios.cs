using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class GrupoDeUsuarios : EventSourced
    {
        public GrupoDeUsuarios()
        {
            this.On<NuevoGrupoCreado>(e => this.StreamName = e.IdGrupo);
        }
    }
}
