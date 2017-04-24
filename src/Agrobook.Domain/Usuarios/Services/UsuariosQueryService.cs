using Agrobook.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosQueryService
    {
        private readonly Func<UsuariosDbContext> contextFactory;

        public UsuariosQueryService(Func<UsuariosDbContext> contextFactory)
        {
            Ensure.NotNull(contextFactory, nameof(contextFactory));

            this.contextFactory = contextFactory;
        }

        public async Task<IList<UsuarioInfoBasica>> ObtenerTodosLosUsuarios()
        {
            using (var context = this.contextFactory())
            {
                var lista = await context.Usuarios.Select(u => new UsuarioInfoBasica
                {
                    Nombre = u.NombreDeUsuario,
                    NombreParaMostrar = u.NombreParaMostrar,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();

                return lista;
            }
        }

        public async Task<UsuarioInfoBasica> ObtenerUsuarioInfoBasica(string usuario)
        {
            using (var context = this.contextFactory())
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
            }
        }
    }
}
