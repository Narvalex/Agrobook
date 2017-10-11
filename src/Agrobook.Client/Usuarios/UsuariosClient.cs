using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Eventing.Client.Http;
using System;
using System.Threading.Tasks;

namespace Agrobook.Client.Usuarios
{
    public class UsuariosClient : ClientBase
    {
        public UsuariosClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "usuarios") { }

        public async Task CrearNuevoUsuario(UsuarioDto dto)
        {
            var command = new CrearNuevoUsuario(null, dto.NombreDeUsuario, dto.NombreParaMostrar, dto.AvatarUrl, dto.Password, dto.Claims);
            await base.Post("crear-nuevo-usuario", command);
        }

        public async Task ActualizarPerfil(ActualizarPerfilDto dto)
        {
            var command = new ActualizarPerfil(null, dto.Usuario, dto.AvatarUrl, dto.NombreParaMostrar, dto.PasswordActual, dto.NuevoPassword);
            await base.Post("actualizar-perfil", command);
        }

        public async Task ResetearPassword(string usuario)
        {
            var command = new ResetearPassword(null, usuario);
            await base.Post("resetear-password", command);
        }

        public async Task<OrganizacionDto> CrearNuevaOrganización(string nombreOrg)
        {
            var command = new CrearNuevaOrganizacion(null, nombreOrg);
            var dto = await base.Post<CrearNuevaOrganizacion, OrganizacionDto>("crear-nueva-organizacion", command);
            return dto;
        }

        public async Task AgregarUsuarioALaOrganizacion(string idUsuario, string idOrganizacion)
        {
            var command = new AgregarUsuarioALaOrganizacion(null, idOrganizacion, idUsuario);
            await base.Post("agregar-usuario-a-la-organizacion", command);
        }

        public async Task OtorgarPermisoAUsuario(string idUsuario, string permiso)
        {
            var command = new OtorgarPermiso(null, idUsuario, permiso);
            await base.Post("otorgar-permiso", command);
        }

        public async Task RetirarPermiso(string usuario, string permiso)
        {
            var command = new RetirarPermiso(null, usuario, permiso);
            await base.Post("retirar-permiso", command);
        }

        public async Task RemoverUsuarioDeOrganizacion(RemoverUsuarioDeOrganizacion cmd)
        {
            await base.Post("remover-usuario-de-organizacion", cmd);
        }
    }
}
