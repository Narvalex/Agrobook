using Agrobook.Domain.Usuarios;
using System;
using System.Threading.Tasks;

namespace Agrobook.Client.Usuarios
{
    public class UsuariosClient : ClientBase<UsuariosClient>
    {
        private readonly string prefix = "usuarios/";

        public UsuariosClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider) { }

        public async Task CrearNuevoUsuario(UsuarioDto dto)
        {
            var command = new CrearNuevoUsuario(null, dto.NombreDeUsuario, dto.NombreParaMostrar, dto.AvatarUrl, dto.Password);
            await base.Post(this.prefix + "crear-nuevo-usuario", dto);
        }
    }
}
