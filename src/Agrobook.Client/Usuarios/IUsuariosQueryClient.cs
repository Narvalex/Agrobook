using Agrobook.Domain.Usuarios.Login;
using Agrobook.Domain.Usuarios.Services;
using Eventing.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agrobook.Client.Usuarios
{
    public interface IUsuariosQueryClient : ISecuredClient
    {
        Task<IList<Claim>> ObtenerClaims();
        Task<IList<Claim>> ObtenerClaims(string idUsuario);
        Task<UsuarioInfoBasica> ObtenerInfoBasicaDeUsuario(string usuario);
        Task<IList<UsuarioInfoBasica>> ObtenerListaDeTodosLosUsuarios();
        Task<IList<OrganizacionDto>> ObtenerOrganizaciones();
        Task<IList<OrganizacionDto>> ObtenerOrganizaciones(string usuarioId);
        Task<IList<OrganizacionDto>> ObtenerOrganizacionesMarcadasDelUsuario(string usuarioId);
    }
}