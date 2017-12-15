using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Eventing.Client;
using System.Threading.Tasks;

namespace Agrobook.Client.Usuarios
{
    public interface IUsuariosClient : ISecuredClient
    {
        Task ActualizarPerfil(ActualizarPerfilDto dto);
        Task AgregarUsuarioALaOrganizacion(string idUsuario, string idOrganizacion);
        Task<OrganizacionDto> CrearNuevaOrganización(string nombreOrg);
        Task CrearNuevoUsuario(UsuarioDto dto);
        Task OtorgarPermisoAUsuario(string idUsuario, string permiso);
        Task ResetearPassword(string usuario);
        Task RetirarPermiso(string usuario, string permiso);
        Task Send(CambiarNombreDeOrganizacion cmd);
        Task Send(EliminarOrganizacion cmd);
        Task Send(RemoverUsuarioDeOrganizacion cmd);
        Task Send(RestaurarOrganizacion cmd);
    }
}