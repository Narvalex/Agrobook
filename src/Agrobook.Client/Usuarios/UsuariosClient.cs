using Agrobook.Domain.Usuarios;
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

        public async Task CrearNuevaOrganización(string nombreOrg)
        {
            var command = new CrearNuevaOrganizacion(null, nombreOrg);
            await base.Post("crear-nueva-organizacion", command);
        }

        public async Task CrearNuevoGrupo(string idOrg, string displayGrupoName)
        {
            var command = new CrearNuevoGrupo(null, idOrg, displayGrupoName);
            await base.Post("crear-nuevo-grupo", command);
        }
    }
}
