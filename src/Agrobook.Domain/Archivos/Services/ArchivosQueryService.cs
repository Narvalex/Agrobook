using Agrobook.Core;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosQueryService : AgrobookQueryService
    {
        public ArchivosQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader reader)
            : base(contextFactory, reader)
        { }

        public async Task<IList<ProductorDto>> ObtenerProductores()
        {
            return await this.QueryAsync(async context =>
            {
                var productores = await context.Usuarios.Where(u => u.EsProductor).ToListAsync();

                // I Really need a document db here...
                var organizaciones = await context.OrganizacionesDeUsuarios.ToListAsync();
                var grupos = await context.GruposDeUsuarios.ToListAsync();

                var list = productores.Select(p =>
                new ProductorDto
                {
                    Id = p.Id,
                    Display = p.Display,
                    AvatarUrl = p.AvatarUrl,
                    Organizaciones = organizaciones
                    .Where(o => o.UsuarioId == p.Id)
                    .Select(o =>
                    new OrganizacionDto
                    {
                        // Display no mas por que el usuario no tendria por que saber el id
                        Display = o.OrganizacionDisplay,
                        Grupos = armarListaDeGrupos(grupos, o.OrganizacionId, p.Id)
                    })
                    .ToArray()
                })
                .ToList();

                return list;
            });

            string armarListaDeGrupos(List<GrupoDeUsuarioEntity> grupos, string orgId, string usuarioId)
            {
                var sb = new StringBuilder();
                grupos
                .Where(g => g.OrganizacionId == orgId && g.UsuarioId == usuarioId)
                .Select(g => g.GrupoDisplay)
                .ToList()
                .ForEach(g =>
                {
                    if (g != UsuariosConstants.DefaultGrupoDisplayName)
                        sb.Append(g + " ");
                });

                return sb.ToString();
            }
        }

        public async Task<IList<ArchivoDto>> ObtenerArchivosDelProductor(string idProductor)
        {
            return await this.QueryAsync(async context =>
            {
                var rawlist = await context.Archivos.Where(x => x.IdProductor == idProductor)
                .ToArrayAsync();

                return rawlist.Select(x =>
                new ArchivoDto
                {
                    Nombre = x.Nombre,
                    Extension = x.Extension,
                    Fecha = x.Fecha.ToShortDateString(),
                    Desc = x.Descripcion
                })
                .ToList();
            });
        }
    }
}
