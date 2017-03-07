using Agrobook.Core;
using Agrobook.Domain.Common;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios
{
    public class UsuariosYGruposService : EventSourcedService
    {
        public const string UsuarioAdmin = "admin";
        public const string DefaultPassword = "changeit";
        private readonly IOneWayEncryptor encryptor;

        public UsuariosYGruposService(
            IEventSourcedRepository repository,
            IDateTimeProvider dateTime,
            IOneWayEncryptor encryptor)
            : base(repository, dateTime)
        {
            Ensure.NotNull(encryptor, nameof(encryptor));

            this.encryptor = encryptor;
        }

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
            admin.Emit(new NuevoUsuarioCreado(new Metadatos("system", this.dateTime.Now), UsuarioAdmin, this.encryptor.Encrypt(DefaultPassword)));
            await this.repository.SaveAsync(admin);
        }

        public async Task HandleAsync(CrearNuevoUsuario cmd)
        {
            var state = new Usuario();
            state.Emit(new NuevoUsuarioCreado(cmd.Metadatos, cmd.Usuario, cmd.PasswordCrudo));
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

            var passwordIngresadoEncriptado = this.encryptor.Encrypt(cmd.PasswordCrudo);
            if (usuario.PasswordEncriptado == passwordIngresadoEncriptado)
                usuario.Emit(new UsuarioInicioSesion(new Metadatos(cmd.Usuario, this.dateTime.Now)));
            else
                return new LoginResult(false);

            await this.repository.SaveAsync(usuario);
            return new LoginResult(true);
        }
    }
}
