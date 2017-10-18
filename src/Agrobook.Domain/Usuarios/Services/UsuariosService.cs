using Agrobook.Common;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Domain.Usuarios.Services;
using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;
using static Agrobook.Domain.Usuarios.UsuariosConstants;

namespace Agrobook.Domain.Usuarios
{
    public static class UsuariosConstants
    {
        public const string UsuarioAdmin = "admin";
        public const string DefaultPassword = "123";
    }

    // TODO: se podria implementar el sistema de clases parciales con esta super clase
    public class UsuariosService : EventSourcedService, ITokenAuthorizationProvider, IProveedorDeFirmaDelUsuario
    {
        private readonly IDateTimeProvider dateTime;
        private readonly IJsonSerializer cryptoSerializer;

        private readonly string adminAvatarUrl;

        public UsuariosService(
            IEventSourcedRepository repository,
            IDateTimeProvider dateTime,
            IJsonSerializer cryptoSerializer,
            string adminAvatarUrl = "../assets/img/avatar/1.png")
            : base(repository)
        {
            Ensure.NotNull(dateTime, nameof(dateTime));
            Ensure.NotNull(cryptoSerializer, nameof(cryptoSerializer));
            Ensure.NotNullOrWhiteSpace(adminAvatarUrl, nameof(adminAvatarUrl));

            this.adminAvatarUrl = adminAvatarUrl;
            this.cryptoSerializer = cryptoSerializer;
            this.dateTime = dateTime;
        }


        public async Task CrearUsuarioAdminAsync()
        {
            var admin = new Usuario();
            var loginInfo = new LoginInfo(UsuarioAdmin, DefaultPassword, new string[] { ClaimDef.Roles.Admin });
            var encryptedLoginInfo = this.EncriptarLoginInfo(loginInfo);
            admin.Emit(new NuevoUsuarioCreado(new Firma("system", this.dateTime.Now), UsuarioAdmin, UsuarioAdmin, this.adminAvatarUrl, encryptedLoginInfo));
            await this.repository.SaveAsync(admin);
        }

        public Firma ObtenerFirmaDelUsuario(string token)
        {
            var tokenInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            var nombreUsuario = tokenInfo.Usuario;

            return new Firma(nombreUsuario, this.dateTime.Now);
        }

        public async Task<Claim[]> ObtenerListaDeClaimsDisponiblesParaElUsuario(string token)
        {
            if (token == null) return null;

            var loginInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);

            var usuarioActualizado = await this.repository.GetOrFailByIdAsync<Usuario>(loginInfo.Usuario);

            //  Refrescamos la info
            loginInfo = this.ExtraerElLoginInfo(usuarioActualizado);

            var claimsPermitidos = ClaimProvider.ObtenerClaimsPermitidosParaCrearNuevoUsuario(loginInfo.Claims);
            return claimsPermitidos;
        }


        public async Task<IList<Claim>> ObtenerClaimsDelUsuario(string idUsuario)
        {
            var usuario = await this.repository.GetOrFailByIdAsync<Usuario>(idUsuario);

            var loginInfo = this.ExtraerElLoginInfo(usuario);
            var claims = ClaimProvider.ObtenerClaimsValidos(loginInfo.Claims).ToList();
            return claims;
        }

        public bool TryAuthorize(string token, params string[] claimsRequired)
        {
            var tokenInfo = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            var nombreUsuario = tokenInfo.Usuario;

            var usuario = this.repository.GetByIdAsync<Usuario>(nombreUsuario).Result;
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

        public LoginInfo GetCurrentUser(string token)
        {
            var currentUser = this.cryptoSerializer.Deserialize<LoginInfo>(token);
            return currentUser;
        }

        public async Task HandleAsync(CrearNuevoUsuario cmd)
        {
            if (cmd.Usuario.Contains(' '))
                throw new ArgumentException("El nombre de usuario no debe contener espacios en blanco");

            var state = new Usuario();
            var loginInfo = new LoginInfo(cmd.Usuario, cmd.PasswordCrudo, cmd.Claims ?? new string[] { ClaimDef.Roles.Invitado });
            var eLoginInfo = this.EncriptarLoginInfo(loginInfo);
            state.Emit(new NuevoUsuarioCreado(cmd.Firma, cmd.Usuario, cmd.NombreParaMostrar, cmd.AvatarUrl, eLoginInfo));
            await this.repository.SaveAsync(state);
        }

        public async Task<LoginResult> HandleAsync(IniciarSesion cmd)
        {
            var usuario = await this.repository.GetByIdAsync<Usuario>(cmd.Usuario);
            if (usuario is null) return LoginResult.Failed;
            LoginInfo loginInfo = this.ExtraerElLoginInfo(usuario);

            if (loginInfo.Password == cmd.PasswordCrudo)
                usuario.Emit(new UsuarioInicioSesion(new Firma(cmd.Usuario, this.dateTime.Now)));
            else
                return LoginResult.Failed;

            await this.repository.SaveAsync(usuario);
            return new LoginResult(true, cmd.Usuario, usuario.NombreParaMostrar, usuario.LoginInfoEncriptado, usuario.AvatarUrl, loginInfo.Claims);
        }

        // Retorna false si no esta autorizado
        public async Task<bool> HandleAsync(ActualizarPerfil cmd)
        {
            var usuario = await this.repository.GetOrFailByIdAsync<Usuario>(cmd.Usuario);
            var usuarioEnEdicion = this.ExtraerElLoginInfo(usuario);

            var autorizado = this.VerificarSiPuedeActualizarPerfil(cmd.ElQueRealizaEstaAccion, usuarioEnEdicion);
            if (!autorizado) return false;

            if (cmd.AvatarUrl != null && usuario.AvatarUrl != cmd.AvatarUrl)
                usuario.Emit(new AvatarUrlActualizado(cmd.Firma, cmd.Usuario, cmd.AvatarUrl));

            if (cmd.NombreParaMostrar != null && usuario.NombreParaMostrar != cmd.NombreParaMostrar)
                usuario.Emit(new NombreParaMostrarActualizado(cmd.Firma, cmd.Usuario, cmd.NombreParaMostrar));

            if (cmd.NuevoPassword != null)
            {
                // Intento de cambiar password detectado
                usuarioEnEdicion = this.ExtraerElLoginInfo(usuario);
                if (cmd.PasswordActual != usuarioEnEdicion.Password)
                    throw new InvalidOperationException($"El password ingresado no es válido. La atualización del perfil se ha cancelado.");

                usuarioEnEdicion.ActualizarPassword(cmd.NuevoPassword);
                var encriptado = this.EncriptarLoginInfo(usuarioEnEdicion);
                usuario.Emit(new PasswordCambiado(cmd.Firma, cmd.Usuario, encriptado));
            }

            await this.repository.SaveAsync(usuario);
            return true;
        }

        public async Task HandleAsync(AgregarUsuarioALaOrganizacion cmd)
        {
            var org = await this.repository.GetOrFailByIdAsync<Organizacion>(cmd.OrganizacionId);

            if (org.LaOrganizacionNoTieneTodaviaUsuarios)
            {
                org.Emit(new UsuarioAgregadoALaOrganizacion(cmd.Firma, cmd.OrganizacionId, cmd.UsuarioId));
            }
            else
            {
                if (org.TieneAlUsuarioComoMiembro(cmd.UsuarioId))
                    throw new InvalidOperationException("El usuario ya pertenece a la organización");

                org.Emit(new UsuarioAgregadoALaOrganizacion(cmd.Firma, cmd.OrganizacionId, cmd.UsuarioId));
            }

            await this.repository.SaveAsync(org);
        }

        public async Task HandleAsync(RemoverUsuarioDeOrganizacion cmd)
        {
            var org = await this.repository.GetOrFailByIdAsync<Organizacion>(cmd.IdOrganizacion);

            if (!org.TieneAlUsuarioComoMiembro(cmd.IdUsuario))
                throw new InvalidOperationException("El usuario ni siquiera pertenece a la organización como para ser removido.");

            org.Emit(new UsuarioRemovidoDeLaOrganizacion(cmd.Firma, cmd.IdUsuario, cmd.IdOrganizacion));

            await this.repository.SaveAsync(org);
        }

        // return false si no esta autorizado
        public async Task<bool> HandleAsync(ResetearPassword cmd)
        {
            var usuario = await this.repository.GetOrFailByIdAsync<Usuario>(cmd.Usuario);
            var loginInfo = this.ExtraerElLoginInfo(usuario);

            var autorizado = this.VerificarSiPuedeActualizarPerfil(cmd.ElQueRealizaEstaAccion, loginInfo);
            if (!autorizado) return false;

            loginInfo.ActualizarPassword(DefaultPassword);
            var encriptado = this.EncriptarLoginInfo(loginInfo);
            usuario.Emit(new PasswordReseteado(cmd.Firma, cmd.Usuario, encriptado));

            await this.repository.SaveAsync(usuario);
            return true;
        }

        public async Task<OrganizacionDto> HandleAsync(CrearNuevaOrganizacion cmd)
        {
            var organizacion = new Organizacion();

            var nombreFormateadoParaDisplay = cmd.NombreCrudo.Trim();
            var nombreFormateadoParaId = cmd.NombreCrudo.ToTrimmedAndWhiteSpaceless().ToLowerInvariant();

            organizacion.Emit(new NuevaOrganizacionCreada(cmd.Firma, nombreFormateadoParaId, nombreFormateadoParaDisplay));

            await this.repository.SaveAsync(organizacion);

            return new OrganizacionDto { Id = nombreFormateadoParaId, Display = nombreFormateadoParaDisplay };
        }

        public async Task HandleAsync(EliminarOrganizacion cmd)
        {
            var org = await this.repository.GetOrFailByIdAsync<Organizacion>(cmd.IdOrg);

            if (org.EstaEliminada)
                throw new InvalidOperationException("La organización está luego eliminada como para intentar eliminarla de nuevo");

            org.Emit(new OrganizacionEliminada(cmd.Firma, cmd.IdOrg));
            await this.repository.SaveAsync(org);
        }

        public async Task HandleAsync(RestaurarOrganizacion cmd)
        {
            var org = await this.repository.GetOrFailByIdAsync<Organizacion>(cmd.IdOrg);

            if (!org.EstaEliminada)
                throw new InvalidOperationException("La organización no está luego eliminada como para intentar restaurarla");

            org.Emit(new OrganizacionRestaurada(cmd.Firma, cmd.IdOrg));
            await this.repository.SaveAsync(org);
        }

        public async Task<bool> HandleAsync(RetirarPermiso cmd)
        {
            // verificamos que no se quiera quitar permiso de admin al usuario de nombre admin
            if (cmd.IdUsuario.EqualsIgnoringCase(UsuariosConstants.UsuarioAdmin)
                && cmd.Permiso.EqualsIgnoringCase(ClaimDef.Roles.Admin))
                throw new InvalidOperationException("No se puede retirar el permiso de admin al usuario admin.");

            var usuario = await this.repository.GetOrFailByIdAsync<Usuario>(cmd.IdUsuario);
            var loginInfo = this.ExtraerElLoginInfo(usuario);

            var autorizado = this.VerificarSiPuedeActualizarPerfil(cmd.Autor, loginInfo);
            if (!autorizado) return false;

            // obtengo los roles y permisos actuales del usuario
            var rolesYPermisos = ClaimProvider.ObtenerClaimsValidos(loginInfo.Claims);

            // verificamos que el usuario tenga ese rol o permiso
            if (!rolesYPermisos.Any(x => x.Id == cmd.Permiso))
                throw new InvalidOperationException("El usuario no tiene luego ese permiso o rol");

            // procedemos a retirar el permiso o rol
            loginInfo.RemoverClaim(cmd.Permiso);
            var loginInfoActualizado = this.EncriptarLoginInfo(loginInfo);
            usuario.Emit(new PermisoRetiradoDelUsuario(cmd.Firma, cmd.IdUsuario, cmd.Permiso, loginInfoActualizado));

            // verificamos si tiene algun rol todavia
            var solamenteRoles = ClaimProvider.ObtenerClaimsValidos(loginInfo.Claims).Where(x => x.Tipo == TipoDeClaim.Rol);
            if (!solamenteRoles.Any())
            {
                // si se quedo sin rol entonces verificamos que lo que se quiso quitar no fue por si acaso el rol por defecto
                // que es el de invitado
                if (cmd.Permiso.EqualsIgnoringCase(ClaimDef.Roles.Invitado))
                    throw new InvalidOperationException("No se puede quitar el rol de invitado si es el unico que le queda");

                // o entonces queda como invitado el pobrecito....
                loginInfo.AddClaim(ClaimDef.Roles.Invitado);
                var encriptado = this.EncriptarLoginInfo(loginInfo);
                usuario.Emit(new PermisoOtorgadoAlUsuario(cmd.Firma, cmd.IdUsuario, ClaimDef.Roles.Invitado, encriptado));
            }

            await this.repository.SaveAsync(usuario);
            return true;
        }

        public async Task<bool> HandleAsync(OtorgarPermiso cmd)
        {
            var usuario = await this.repository.GetOrFailByIdAsync<Usuario>(cmd.IdUsuario);
            var loginInfo = this.ExtraerElLoginInfo(usuario);

            var autorizado = this.VerificarSiPuedeActualizarPerfil(cmd.Autor, loginInfo);
            if (!autorizado) return false;

            if (loginInfo.Claims.Any(x => x.EqualsIgnoringCase(cmd.Permiso)))
                throw new InvalidOperationException("El usuario ya tiene ese permiso");

            loginInfo.AddClaim(cmd.Permiso);
            var encriptado = this.EncriptarLoginInfo(loginInfo);
            usuario.Emit(new PermisoOtorgadoAlUsuario(cmd.Firma, cmd.IdUsuario, cmd.Permiso, encriptado));

            await this.repository.SaveAsync(usuario);
            return true;
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

        private bool VerificarSiPuedeActualizarPerfil(LoginInfo autor, LoginInfo objetivo)
        {
            var autorizado = false;
            if (autor.Claims.Any(x => x == Roles.Admin))
                autorizado = true;
            else if (autor.Claims.Any(x => x == Roles.Gerente))
            {
                if (objetivo.Claims.Any(x => x == Roles.Admin))
                    autorizado = false;
                else if (objetivo.Claims.Any(x => x == Roles.Gerente))
                {
                    // esta intentando actualizar su propio perfil el gerente
                    if (objetivo.Usuario == autor.Usuario)
                        autorizado = true;
                    else
                        autorizado = false;
                }
                else
                    autorizado = true;
            }
            else if (autor.Claims.Any(x => x == Roles.Tecnico))
            {
                if (objetivo.Claims.Any(x => x == Roles.Admin || x == Roles.Gerente))
                    autorizado = false;
                else if (objetivo.Claims.Any(x => x == Roles.Tecnico))
                {
                    // esta editando su propio perfil el tecnico
                    if (objetivo.Usuario == autor.Usuario)
                        autorizado = true;
                }
                else
                    autorizado = true;
            }
            else
            {
                // el productor / invitado solo puede actualizar su propio perfil
                if (objetivo.Usuario == autor.Usuario)
                    autorizado = true;
            }
            return autorizado;
        }
        #endregion
    }
}
