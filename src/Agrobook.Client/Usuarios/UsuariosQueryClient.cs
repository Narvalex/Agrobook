using Agrobook.Domain.Usuarios.Login;
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

        public async Task<IList<UsuarioInfoBasica>> ObtenerListaDeTodosLosUsuarios()
        {
            var lista = await base.Get<IList<UsuarioInfoBasica>>("todos");
            return lista;
        }

        public async Task<UsuarioInfoBasica> ObtenerInfoBasicaDeUsuario(string usuario)
        {
            var dto = await base.Get<UsuarioInfoBasica>($"info-basica/{usuario}");
            return dto;
        }

        public async Task<IList<OrganizacionDto>> ObtenerOrganizaciones()
        {
            var dto = await base.Get<IList<OrganizacionDto>>("organizaciones");
            return dto;
        }

        public async Task<IList<Claim>> ObtenerClaims()
        {
            var dto = await base.Get<IList<Claim>>("claims");
            return dto;
        }
    }
}
