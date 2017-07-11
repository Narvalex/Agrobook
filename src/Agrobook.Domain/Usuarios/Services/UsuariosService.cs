using Agrobook.Core;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Domain.Usuarios.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Agrobook.Domain.Usuarios.UsuariosConstants;

namespace Agrobook.Domain.Usuarios
{
    public static class UsuariosConstants
    {
        public const string UsuarioAdmin = "admin";
        public const string DefaultPassword = "123";

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
            var loginInfo = new LoginInfo(UsuarioAdmin, DefaultPassword, new string[] { ClaimDef.Roles.Admin });
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

            var usuarioActualizado = await this.repository.GetOrFailAsync<Usuario>(loginInfo.Usuario);

            //  Refrescamos la info
            loginInfo = this.ExtraerElLoginInfo(usuarioActualizado);

            var claimsPermitidos = ClaimProvider.ObtenerClaimsPermitidosParaCrearNuevoUsuario(loginInfo.Claims);
            return claimsPermitidos;
        }

        public async Task<IList<Claim>> ObtenerClaimsDelUsuario(string idUsuario)
        {
            var usuario = await this.repository.GetOrFailAsync<Usuario>(idUsuario);

            var loginInfo = this.ExtraerElLoginInfo(usuario);
            var claims = ClaimProvider.Transformar(loginInfo.Claims).ToList();
            return claims;
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

            if (tokenInfo.Claims.Any(c => c == ClaimDef.Roles.Admin))
                return true;

            var tienePermiso = tokenInfo.Claims.Any(x => claimsRequired.Any(r => r == x));
            return tienePermiso;
        }

        public string[] GetClaims(string token)
        {
            var tokenInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            return tokenInfo.Claims;
        }

        public async Task HandleAsync(CrearNuevoUsuario cmd)
        {
            if (cmd.Usuario.Contains(' '))
                throw new ArgumentException("El nombre de usuario no debe contener espacios en blanco");

            var state = new Usuario();
            var loginInfo = new LoginInfo(cmd.Usuario, cmd.PasswordCrudo, cmd.Claims ?? new string[] { ClaimDef.Roles.Invitado });
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
            return new LoginResult(true, cmd.Usuario, usuario.NombreParaMostrar, usuario.LoginInfoEncriptado, usuario.AvatarUrl, loginInfo.Claims);
        }

        public async Task HandleAsync(ActualizarPerfil cmd)
        {
            var usuario = await this.repository.GetOrFailAsync<Usuario>(cmd.Usuario);

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
            var org = await this.repository.GetOrFailAsync<Organizacion>(cmd.OrganizacionId);

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
            var usuario = await this.repository.GetOrFailAsync<Usuario>(cmd.Usuario);
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
            if (cmd.GrupoDisplayName.ToLowerTrimmedAndWhiteSpaceless() == UsuariosConstants.DefaultGrupoId)
                throw new InvalidOperationException($"El nombre del grupo no puede ser {cmd.GrupoDisplayName}");

            var org = await this.repository.GetOrFailAsync<Organizacion>(cmd.IdOrganizacion);
            var idGrupo = cmd.GrupoDisplayName.ToLowerTrimmedAndWhiteSpaceless();
            if (org.YaTieneGrupoConId(idGrupo))
                throw new InvalidOperationException($"Ya existe el grupo con id {idGrupo} en la organización {org.NombreParaMostrar}");
            org.Emit(new NuevoGrupoCreado(cmd.Metadatos, idGrupo, cmd.GrupoDisplayName, cmd.IdOrganizacion));
            await this.repository.SaveAsync(org);
            return new GrupoDto { Id = idGrupo, Display = cmd.GrupoDisplayName };
        }

        public async Task HandleAsync(AgregarUsuarioAUnGrupo cmd)
        {
            var org = await this.repository.GetOrFailAsync<Organizacion>(cmd.OrganizacionId);

            if (!org.YaTieneAlUsuarioComoMiembro(cmd.UsuarioId))
                throw new InvalidOperationException("El usuario todavia no es miembro de la organizacion");

            if (!org.YaTieneGrupoConId(cmd.GrupoId))
                throw new InvalidOperationException("No existe el grupo al que se quiere agregar el usuario");

            if (org.YaTieneUsuarioDentroDelGrupo(cmd.GrupoId, cmd.UsuarioId))
                throw new InvalidOperationException("El usuario ya esta dentro del grupo");

            org.Emit(new UsuarioAgregadoAUnGrupo(cmd.Metadatos, cmd.OrganizacionId, cmd.UsuarioId, cmd.GrupoId));

            await this.repository.SaveAsync(org);
        }

        public async Task HandleAsync(RetirarPermiso cmd)
        {
            // verificamos que no se quiera quitar permiso de admin al usuario de nombre admin
            if (cmd.IdUsuario.EqualsIgnoringCase(UsuariosConstants.UsuarioAdmin)
                && cmd.Permiso.EqualsIgnoringCase(ClaimDef.Roles.Admin))
                throw new InvalidOperationException("No se puede retirar el permiso de admin al usuario admin.");

            var usuario = await this.repository.GetOrFailAsync<Usuario>(cmd.IdUsuario);
            var loginInfo = this.ExtraerElLoginInfo(usuario);

            // obtengo los roles y permisos actuales del usuario
            var rolesYPermisos = ClaimProvider.Transformar(loginInfo.Claims);

            // verificamos que el usuario tenga ese rol o permiso
            if (!rolesYPermisos.Any(x => x.Id == cmd.Permiso))
                throw new InvalidOperationException("El usuario no tiene luego ese permiso o rol");

            // procedemos a retirar el permiso o rol
            loginInfo.RemoverClaim(cmd.Permiso);
            var loginInfoActualizado = this.EncriptarLoginInfo(loginInfo);
            usuario.Emit(new PermisoRetiradoDelUsuario(cmd.Metadatos, cmd.IdUsuario, cmd.Permiso, loginInfoActualizado));

            // verificamos si tiene algun rol todavia
            var solamenteRoles = ClaimProvider.ObtenerRoles(loginInfo.Claims);
            if (!solamenteRoles.Any())
            {
                // si se quedo sin rol entonces verificamos que lo que se quiso quitar no fue por si acaso el rol por defecto
                // que es el de invitado
                if (cmd.Permiso.EqualsIgnoringCase(ClaimDef.Roles.Invitado))
                    throw new InvalidOperationException("No se puede quitar el rol de invitado si es el unico que le queda");

                // o entonces queda como invitado el pobrecito....
                loginInfo.AddClaim(ClaimDef.Roles.Invitado);
                var encriptado = this.EncriptarLoginInfo(loginInfo);
                usuario.Emit(new PermisoOtorgadoAlUsuario(cmd.Metadatos, cmd.IdUsuario, ClaimDef.Roles.Invitado, encriptado));
            }

            await this.repository.SaveAsync(usuario);
        }

        public async Task HandleAsync(OtorgarPermiso cmd)
        {
            var usuario = await this.repository.GetOrFailAsync<Usuario>(cmd.IdUsuario);
            var loginInfo = this.ExtraerElLoginInfo(usuario);

            if (loginInfo.Claims.Any(x => x.EqualsIgnoringCase(cmd.Permiso)))
                throw new InvalidOperationException("El usuario ya tiene ese permiso");

            loginInfo.AddClaim(cmd.Permiso);
            var encriptado = this.EncriptarLoginInfo(loginInfo);
            usuario.Emit(new PermisoOtorgadoAlUsuario(cmd.Metadatos, cmd.IdUsuario, cmd.Permiso, encriptado));

            await this.repository.SaveAsync(usuario);
        }

        #region Helpers
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
        #endregion
    }
}
