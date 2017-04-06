using Agrobook.Domain.Usuarios.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agrobook.Client.Usuarios
{
    public class UsuariosQueryClient : ClientBase
    {
        public UsuariosQueryClient(HttpLite http, Func<string> tokenProvider = null) 
            : base(http, tokenProvider, "usuarios/query")
        {
        }

        public async Task<IList<UsuarioDtoBasico>> ObtenerListaDeTodosLosUsuarios()
        {
            var lista = await base.Get<IList<UsuarioDtoBasico>>("todos");
            return lista;
        }
    }
}
