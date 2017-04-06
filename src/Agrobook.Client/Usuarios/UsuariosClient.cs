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
    }
}
