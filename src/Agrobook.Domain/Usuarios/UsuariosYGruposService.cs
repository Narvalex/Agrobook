using Agrobook.Core;
using Agrobook.Domain.Common;

namespace Agrobook.Domain.Usuarios
{
    public class UsuariosYGruposService : EventSourcedService
    {
        public const string UsuarioAdmin = "admin";

        public UsuariosYGruposService(IEventSourcedRepository repository, IDateTimeProvider dateTime)
            : base(repository, dateTime)
        { }

        public bool ExisteUsuarioAdmin
        {
            get
            {
                var usuarioAdmin = this.repository.Get<Usuario>(UsuarioAdmin);
                return usuarioAdmin != null;
            }
        }

        public void CrearUsuarioAdmin()
        {
            var admin = new Usuario();
            admin.Emit(new NuevoUsuarioCreado(new Metadatos("system", this.dateTime.Now), "system", "changeit"));
            this.repository.Save(admin);
        }

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
            var usuario = this.repository.Get<Usuario>(cmd.Usuario);
            if (usuario is null) return new LoginResult(false);

            if (usuario.Password == cmd.Password)
                usuario.Emit(new UsuarioInicioSesion(new Metadatos(cmd.Usuario, this.dateTime.Now)));
            else
                return new LoginResult(false);

            this.repository.Save(usuario);
            return new LoginResult(true);
        }
    }
}
