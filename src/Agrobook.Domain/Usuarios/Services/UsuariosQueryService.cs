using Agrobook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosQueryService : AgrobookQueryService
    {
        public UsuariosQueryService(Func<AgrobookDbContext> contextFactory) 
            : base(contextFactory)
        { }

        public async Task<IList<UsuarioInfoBasica>> ObtenerTodosLosUsuarios()
        {
            return await this.QueryAsync(async context =>
            {
                var lista = await context.Usuarios.Select(u => new UsuarioInfoBasica
                {
                    Nombre = u.NombreDeUsuario,
                    NombreParaMostrar = u.NombreParaMostrar,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();

                return lista;
            });
        }

        public async Task<UsuarioInfoBasica> ObtenerUsuarioInfoBasica(string usuario)
        {
            return await this.QueryAsync(async context =>
            {
                var dto = await context
                                .Usuarios
                                .Where(u => u.NombreDeUsuario == usuario)
                                .Select(u => new UsuarioInfoBasica
                                {
                                    Nombre = u.NombreDeUsuario,
                                    NombreParaMostrar = u.NombreParaMostrar,
                                    AvatarUrl = u.AvatarUrl
                                })
                                .SingleOrDefaultAsync();

                return dto;
            });
        }
    }
}
