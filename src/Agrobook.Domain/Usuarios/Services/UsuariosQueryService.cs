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

        public async Task<IList<UsuarioDtoBasico>> ObtenerTodosLosUsuarios()
        {
            using (var context = this.contextFactory())
            {
                var lista = await context.Usuarios.Select(u => new UsuarioDtoBasico
                {
                    Nombre = u.NombreDeUsuario,
                    NombreCompleto = u.NombreCompleto,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();

                return lista;
            }
        }
    }
}
