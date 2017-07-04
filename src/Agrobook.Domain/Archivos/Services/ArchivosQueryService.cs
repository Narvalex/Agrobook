using Agrobook.Core;
using Agrobook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                return await context.Usuarios.Where(u => u.EsProductor).Select(u =>
                new ProductorDto
                {
                    Id = u.Id,
                    Display = u.Display,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();
            });
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
