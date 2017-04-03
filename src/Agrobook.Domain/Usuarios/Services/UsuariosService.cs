using Agrobook.Core;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios
{
    public class UsuariosService : EventSourcedService, ITokenAuthorizationProvider
    {
        public const string UsuarioAdmin = "admin";
        public const string DefaultPassword = "changeit";
        private readonly IJsonSerializer cryptoSerializer;
        private readonly string adminAvatarUrl;

        public UsuariosService(
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
            var loginInfo = new LoginInfo(cmd.Usuario, cmd.PasswordCrudo, cmd.Claims ?? new string[0]);
            var eLoginInfo = this.cryptoSerializer.Serialize(loginInfo);
            state.Emit(new NuevoUsuarioCreado(cmd.Metadatos, cmd.Usuario, cmd.NombreParaMostrar, cmd.AvatarUrl, eLoginInfo));
            await this.repository.SaveAsync(state);
        }

        public async Task<LoginResult> HandleAsync(IniciarSesion cmd)
        {
            var usuario = await this.repository.GetAsync<Usuario>(cmd.Usuario);
            if (usuario is null) return LoginResult.Failed;

            var loginInfo = this.cryptoSerializer.Deserialize<LoginInfo>(usuario.LoginInfoEncriptado);

            if (loginInfo.Password == cmd.PasswordCrudo)
                usuario.Emit(new UsuarioInicioSesion(new Metadatos(cmd.Usuario, this.dateTime.Now)));
            else
                return LoginResult.Failed;

            await this.repository.SaveAsync(usuario);
            return new LoginResult(true, usuario.NombreParaMostrar, usuario.LoginInfoEncriptado, usuario.AvatarUrl);
        }

        public bool TryAuthorize(string token, params string[] claimsRequired)
        {
            var tokenInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            var nombreUsuario = tokenInfo.Usuario;

            var usuario = this.repository.GetAsync<Usuario>(nombreUsuario).Result;
            if (usuario == null)
                return false;

            // Validar token
            if (token != usuario.LoginInfoEncriptado)
                return false;

            if (tokenInfo.Claims.Any(c => c == Claims.Roles.Admin))
                return true;

            var tienePermiso = tokenInfo.Claims.Any(x => claimsRequired.Any(r => r == x));
            return tienePermiso;
        }
    }
}
