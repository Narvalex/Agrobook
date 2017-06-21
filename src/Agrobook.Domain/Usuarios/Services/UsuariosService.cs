using Agrobook.Core;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Domain.Usuarios.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Agrobook.Domain.Usuarios.UsuariosConstants;

namespace Agrobook.Domain.Usuarios
{
    public static class UsuariosConstants
    {
        public const string UsuarioAdmin = "admin";
        public const string DefaultPassword = "1234";

        public const string DefaultGrupoId = "todos";
        public const string DefaultGrupoDisplayName = "Todos";
    }

    public class UsuariosService : EventSourcedService, ITokenAuthorizationProvider, IProveedorDeMetadatosDelUsuario
    {
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


        public async Task CrearUsuarioAdminAsync()
        {
            var admin = new Usuario();
            var loginInfo = new LoginInfo(UsuarioAdmin, DefaultPassword, new string[] { ClaimsDefs.Roles.Admin });
            var encryptedLoginInfo = this.EncriptarLoginInfo(loginInfo);
            admin.Emit(new NuevoUsuarioCreado(new Metadatos("system", this.dateTime.Now), UsuarioAdmin, UsuarioAdmin, this.adminAvatarUrl, encryptedLoginInfo));
            await this.repository.SaveAsync(admin);
        }

        public Metadatos ObtenerMetadatosDelUsuario(string token)
        {
            var tokenInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            var nombreUsuario = tokenInfo.Usuario;

            return new Metadatos(nombreUsuario, this.dateTime.Now);
        }

        public async Task<Claim[]> ObtenerListaDeClaimsDisponiblesParaElUsuario(string token)
        {
            if (token == null) return null;

            var loginInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);

            var usuarioActualizado = await this.IntentarRecuperarUsuarioAsync(loginInfo.Usuario);

            //  Refrescamos la info
            loginInfo = this.ExtraerElLoginInfo(usuarioActualizado);

            var claimsPermitidos = ClaimProvider.ObtenerClaimsPermitidosParaCrearNuevoUsuario(loginInfo.Claims);
            return claimsPermitidos;
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

            if (tokenInfo.Claims.Any(c => c == ClaimsDefs.Roles.Admin))
                return true;

            var tienePermiso = tokenInfo.Claims.Any(x => claimsRequired.Any(r => r == x));
            return tienePermiso;
        }

        public async Task HandleAsync(CrearNuevoUsuario cmd)
        {
            ValidarQue.ElNombreDeUsuarioNoContengaEspaciosEnBlanco(cmd.Usuario);

            var state = new Usuario();
            var loginInfo = new LoginInfo(cmd.Usuario, cmd.PasswordCrudo, cmd.Claims ?? new string[0]);
            var eLoginInfo = this.EncriptarLoginInfo(loginInfo);
            state.Emit(new NuevoUsuarioCreado(cmd.Metadatos, cmd.Usuario, cmd.NombreParaMostrar, cmd.AvatarUrl, eLoginInfo));
            await this.repository.SaveAsync(state);
        }

        public async Task<LoginResult> HandleAsync(IniciarSesion cmd)
        {
            var usuario = await this.repository.GetAsync<Usuario>(cmd.Usuario);
            if (usuario is null) return LoginResult.Failed;
            LoginInfo loginInfo = this.ExtraerElLoginInfo(usuario);

            if (loginInfo.Password == cmd.PasswordCrudo)
                usuario.Emit(new UsuarioInicioSesion(new Metadatos(cmd.Usuario, this.dateTime.Now)));
            else
                return LoginResult.Failed;

            await this.repository.SaveAsync(usuario);
            return new LoginResult(true, cmd.Usuario, usuario.NombreParaMostrar, usuario.LoginInfoEncriptado, usuario.AvatarUrl);
        }

        public async Task HandleAsync(ActualizarPerfil cmd)
        {
            var usuario = await IntentarRecuperarUsuarioAsync(cmd.Usuario);

            if (cmd.AvatarUrl != null && usuario.AvatarUrl != cmd.AvatarUrl)
                usuario.Emit(new AvatarUrlActualizado(cmd.Metadatos, cmd.Usuario, cmd.AvatarUrl));

            if (cmd.NombreParaMostrar != null && usuario.NombreParaMostrar != cmd.NombreParaMostrar)
                usuario.Emit(new NombreParaMostrarActualizado(cmd.Metadatos, cmd.Usuario, cmd.NombreParaMostrar));

            if (cmd.NuevoPassword != null)
            {
                // Intento de cambiar password detectado
                var loginInfo = this.ExtraerElLoginInfo(usuario);
                if (cmd.PasswordActual != loginInfo.Password)
                    throw new InvalidOperationException($"El password ingresado no es válido. La atualización del perfil se ha cancelado.");

                loginInfo.ActualizarPassword(cmd.NuevoPassword);
                var encriptado = this.EncriptarLoginInfo(loginInfo);
                usuario.Emit(new PasswordCambiado(cmd.Metadatos, cmd.Usuario, encriptado));
            }

            await this.repository.SaveAsync(usuario);
        }

        public async Task HandleAsync(AgregarUsuarioALaOrganizacion cmd)
        {
            var org = await this.IntentarRecuperarOrganizacionAsync(cmd.OrganizacionId);

            if (org.LaOrganizacionNoTieneTodaviaUsuarios)
            {
                org.Emit(new UsuarioAgregadoALaOrganizacion(cmd.Metadatos, cmd.OrganizacionId, cmd.UsuarioId));
                org.Emit(new NuevoGrupoCreado(cmd.Metadatos, DefaultGrupoId, DefaultGrupoDisplayName, cmd.OrganizacionId));
                org.Emit(new UsuarioAgregadoAUnGrupo(cmd.Metadatos, cmd.OrganizacionId, cmd.UsuarioId, DefaultGrupoId));
            }
            else
            {
                if (org.YaTieneAlUsuarioComoMiembro(cmd.UsuarioId))
                    throw new InvalidOperationException("El usuario ya pertenece a la organización");

                org.Emit(new UsuarioAgregadoALaOrganizacion(cmd.Metadatos, cmd.OrganizacionId, cmd.UsuarioId));
                org.Emit(new UsuarioAgregadoAUnGrupo(cmd.Metadatos, cmd.OrganizacionId, cmd.UsuarioId, UsuariosConstants.DefaultGrupoId));
            }

            await this.repository.SaveAsync(org);
        }

        public async Task HandleAsync(ResetearPassword cmd)
        {
            var usuario = await this.IntentarRecuperarUsuarioAsync(cmd.Usuario);
            var loginInfo = this.ExtraerElLoginInfo(usuario);
            loginInfo.ActualizarPassword(DefaultPassword);
            var encriptado = this.EncriptarLoginInfo(loginInfo);
            usuario.Emit(new PasswordReseteado(cmd.Metadatos, cmd.Usuario, encriptado));

            await this.repository.SaveAsync(usuario);
        }

        public async Task<OrganizacionDto> HandleAsync(CrearNuevaOrganizacion cmd)
        {
            var organizacion = new Organizacion();

            var nombreFormateadoParaDisplay = cmd.NombreCrudo.Trim();
            var nombreFormateadoParaId = cmd.NombreCrudo.ToLowerTrimmedAndWhiteSpaceless();

            organizacion.Emit(new NuevaOrganizacionCreada(cmd.Metadatos, nombreFormateadoParaId, nombreFormateadoParaDisplay));

            await this.repository.SaveAsync(organizacion);

            return new OrganizacionDto { Id = nombreFormateadoParaId, Display = nombreFormateadoParaDisplay };
        }

        public async Task<GrupoDto> HandleAsync(CrearNuevoGrupo cmd)
        {
            ValidarQue.ElNombreDelGrupoSeLlameIgualAlPorDefecto(cmd.GrupoDisplayName);

            var org = await this.IntentarRecuperarOrganizacionAsync(cmd.IdOrganizacion);
            var idGrupo = cmd.GrupoDisplayName.ToLowerTrimmedAndWhiteSpaceless();
            if (org.YaTieneGrupoConId(idGrupo))
                throw new InvalidOperationException($"Ya existe el grupo con id {idGrupo} en la organización {org.NombreParaMostrar}");
            org.Emit(new NuevoGrupoCreado(cmd.Metadatos, idGrupo, cmd.GrupoDisplayName, cmd.IdOrganizacion));
            await this.repository.SaveAsync(org);
            return new GrupoDto { Id = idGrupo, Display = cmd.GrupoDisplayName };
        }

        public async Task HandleAsync(AgregarUsuarioAUnGrupo cmd)
        {
            var org = await this.IntentarRecuperarOrganizacionAsync(cmd.OrganizacionId);

            if (!org.YaTieneAlUsuarioComoMiembro(cmd.UsuarioId))
                throw new InvalidOperationException("El usuario todavia no es miembro de la organizacion");

            if (!org.YaTieneGrupoConId(cmd.GrupoId))
                throw new InvalidOperationException("No existe el grupo al que se quiere agregar el usuario");

            if (org.YaTieneUsuarioDentroDelGrupo(cmd.GrupoId, cmd.UsuarioId))
                throw new InvalidOperationException("El usuario ya esta dentro del grupo");

            org.Emit(new UsuarioAgregadoAUnGrupo(cmd.Metadatos, cmd.OrganizacionId, cmd.UsuarioId, cmd.GrupoId));

            await this.repository.SaveAsync(org);
        }

        private async Task<Usuario> IntentarRecuperarUsuarioAsync(string usuario)
        {
            var state = await this.repository.GetAsync<Usuario>(usuario);
            if (state is null) throw new InvalidOperationException($"El usuario {usuario} no existe");
            return state;
        }

        private async Task<Organizacion> IntentarRecuperarOrganizacionAsync(string id)
        {
            var state = await this.repository.GetAsync<Organizacion>(id);
            if (state is null) throw new InvalidOperationException($"La organización {id} no existe");
            return state;
        }

        private string EncriptarLoginInfo(LoginInfo loginInfo)
        {
            var encriptado = this.cryptoSerializer.Serialize(loginInfo);
            return encriptado;
        }

        private LoginInfo ExtraerElLoginInfo(Usuario usuario)
        {
            var info = this.cryptoSerializer.Deserialize<LoginInfo>(usuario.LoginInfoEncriptado);
            return info;
        }

        public static class ValidarQue
        {
            public static void ElNombreDeUsuarioNoContengaEspaciosEnBlanco(string nombreDeUsuario)
            {
                if (nombreDeUsuario.Contains(' '))
                    throw new ArgumentException("El nombre de usuario no debe contener espacios en blanco");
            }

            public static void ElNombreDelGrupoSeLlameIgualAlPorDefecto(string nombrePropuestoDelGrupo)
            {
                if (nombrePropuestoDelGrupo.ToLowerTrimmedAndWhiteSpaceless() == UsuariosConstants.DefaultGrupoId)
                    throw new InvalidOperationException($"El nombre del grupo no puede ser {nombrePropuestoDelGrupo}");
            }
        }
    }
}
