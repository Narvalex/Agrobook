using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class UsuariosYGruposService : EventSourcedService
    {
        public UsuariosYGruposService(IEventSourcedRepository repository)
            : base(repository)
        { }

        public void Handle(CrearGrupo cmd)
        {
            var state = new GrupoDeUsuarios();
            state.Emit(new NuevoGrupoCreado(cmd.IdGrupo));
            this.repository.Save(state);
        }

        public void Handle(AgregarUsuarioAGrupo cmd)
        {
            var state = this.repository.Get<GrupoDeUsuarios>(cmd.IdGrupo);
            if (state.YaPerteneceUsuarioAlGrupo(cmd.IdUsuario))
                return;

            state.Emit(new UsuarioAgregadoAGrupo(cmd.IdGrupo, cmd.IdUsuario));
            this.repository.Save(state);
        }
    }
}
