using Agrobook.Core;
using Agrobook.Domain.Common;
using System.Threading.Tasks;

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
                var usuarioAdmin = this.repository.GetAsync<Usuario>(UsuarioAdmin).Result;
                return usuarioAdmin != null;
            }
        }

        public async Task CrearUsuarioAdminAsync()
        {
            var admin = new Usuario();
            admin.Emit(new NuevoUsuarioCreado(new Metadatos("system", this.dateTime.Now), "admin", "changeit"));
            await this.repository.SaveAsync(admin);
        }

        public async Task HandleAsync(CrearNuevoUsuario cmd)
        {
            var state = new Usuario();
            state.Emit(new NuevoUsuarioCreado(cmd.Metadatos, cmd.Usuario, cmd.Password));
            await this.repository.SaveAsync(state);
        }

        public async Task HandleAsync(CrearGrupo cmd)
        {
            var state = new GrupoDeUsuarios();
            state.Emit(new NuevoGrupoCreado(cmd.Metadatos, cmd.IdGrupo));
            await this.repository.SaveAsync(state);
        }

        public async Task HandleAsync(AgregarUsuarioAGrupo cmd)
        {
            var state = await this.repository.GetAsync<GrupoDeUsuarios>(cmd.IdGrupo);
            if (state.YaPerteneceUsuarioAlGrupo(cmd.IdUsuario))
                return;

            state.Emit(new UsuarioAgregadoAGrupo(cmd.Metadatos, cmd.IdGrupo, cmd.IdUsuario));
            await this.repository.SaveAsync(state);
        }

        public async Task<LoginResult> HandleAsync(IniciarSesion cmd)
        {
            var usuario = await this.repository.GetAsync<Usuario>(cmd.Usuario);
            if (usuario is null) return new LoginResult(false);

            if (usuario.Password == cmd.Password)
                usuario.Emit(new UsuarioInicioSesion(new Metadatos(cmd.Usuario, this.dateTime.Now)));
            else
                return new LoginResult(false);

            await this.repository.SaveAsync(usuario);
            return new LoginResult(true);
        }
    }
}
