using Agrobook.Core;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios
{
    public class UsuariosYGruposService : EventSourcedService, ITokenAuthorizationProvider
    {
        public const string UsuarioAdmin = "admin";
        public const string DefaultPassword = "changeit";
        private readonly IJsonSerializer cryptoSerializer;
        private readonly string adminAvatarUrl;

        public UsuariosYGruposService(
            IEventSourcedRepository repository,
            IDateTimeProvider dateTime,
            IJsonSerializer cryptoSerializer,
            string adminAvatarUrl = "./assets/img/avatar/1.png")
            : base(repository, dateTime)
        {
            Ensure.NotNull(cryptoSerializer, nameof(cryptoSerializer));
            Ensure.NotNullOrWhiteSpace(adminAvatarUrl, nameof(adminAvatarUrl));

            this.adminAvatarUrl = adminAvatarUrl;
            this.cryptoSerializer = cryptoSerializer;
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
            var loginInfo = new LoginInfo(UsuarioAdmin, DefaultPassword, new string[] { Claims.Roles.Admin });
            var encryptedLoginInfo = this.cryptoSerializer.Serialize(loginInfo);
            admin.Emit(new NuevoUsuarioCreado(new Metadatos("system", this.dateTime.Now), UsuarioAdmin, UsuarioAdmin, this.adminAvatarUrl, encryptedLoginInfo));
            await this.repository.SaveAsync(admin);
        }

        public async Task HandleAsync(CrearNuevoUsuario cmd)
        {
            var state = new Usuario();
            state.Emit(new NuevoUsuarioCreado(cmd.Metadatos, cmd.Usuario, cmd.NombreParaMostrar, cmd.AvatarUrl, cmd.PasswordCrudo));
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
            if (usuario is null) return new LoginResult(false, null, null);

            var loginInfo = this.cryptoSerializer.Deserialize<LoginInfo>(usuario.LoginInfoEncriptado);

            if (loginInfo.Password == cmd.PasswordCrudo)
                usuario.Emit(new UsuarioInicioSesion(new Metadatos(cmd.Usuario, this.dateTime.Now)));
            else
                return new LoginResult(false, null, null);

            await this.repository.SaveAsync(usuario);
            return new LoginResult(true, usuario.NombreParaMostrar, usuario.LoginInfoEncriptado);
        }

        public bool TryAuthorize(string token, params string[] claimsRequired)
        {
            var tokenInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            var nombreUsuario = tokenInfo.Usuario;

            var usuario = this.repository.GetAsync<Usuario>(nombreUsuario).Result;
            if (usuario == null)
                return false;

            var realUserInfo = this.cryptoSerializer.Deserialize<LoginInfo>(usuario.LoginInfoEncriptado);
            var tienePermiso = realUserInfo.Claims.Any(x => claimsRequired.Any(r => r == x));
            return tienePermiso;
        }
    }
}
