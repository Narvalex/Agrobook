using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    public class UsuariosYGruposService : EventSourcedService
    {
        public UsuariosYGruposService(IEventSourcedRepository repository)
            : base(repository)
        { }

        public void Handle(CrearNuevoUsuario cmd)
        {
            var state = new Usuario();
            state.Emit(new NuevoUsuarioCreado(cmd.Metadatos, cmd.Usuario, cmd.Password));
            this.repository.Save(state);
        }

        public void Handle(CrearGrupo cmd)
        {
            var state = new GrupoDeUsuarios();
            state.Emit(new NuevoGrupoCreado(cmd.Metadatos, cmd.IdGrupo));
            this.repository.Save(state);
        }

        public void Handle(AgregarUsuarioAGrupo cmd)
        {
            var state = this.repository.Get<GrupoDeUsuarios>(cmd.IdGrupo);
            if (state.YaPerteneceUsuarioAlGrupo(cmd.IdUsuario))
                return;

            state.Emit(new UsuarioAgregadoAGrupo(cmd.Metadatos, cmd.IdGrupo, cmd.IdUsuario));
            this.repository.Save(state);
        }

        public LoginResult Handle(IniciarSesion cmd)
        {
            var state = this.repository.Get<Usuario>(cmd.Usuario);
            if (state is null) return new LoginResult(false);

            throw new System.NotImplementedException();
        }
    }
}
