using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class GrupoDeUsuariosService : CommandHandler
    {
        public GrupoDeUsuariosService(IEventSourcedRepository repository)
            : base(repository)
        { }

        public void Handle(CrearGrupo cmd)
        {
            var state = new GrupoDeUsuarios();
            state.Update(new NuevoGrupoCreado(cmd.IdGrupo));
            this.repository.Persist(state);
        }
    }
}
