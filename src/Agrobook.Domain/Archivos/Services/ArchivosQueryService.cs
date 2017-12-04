using Agrobook.Domain.Common;
using Eventing.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosQueryService : DbContextQueryService<AgrobookDbContext>
    {
        public ArchivosQueryService(Func<AgrobookDbContext> contextFactory)
            : base(contextFactory)
        { }

        public async Task<IList<MetadatosDeArchivo>> ObtenerArchivos(string idColeccion)
        {
            return await this.QueryAsync(async context =>
            {
                var rawList = await context.Archivos.Where(x => x.IdColeccion == idColeccion)
                .ToArrayAsync();

                var list = rawList.Select(x =>
                new MetadatosDeArchivo
                {
                    Nombre = x.Nombre,
                    Extension = x.Extension,
                    Size = x.Size,
                    Tipo = x.Tipo,
                    Fecha = x.Fecha,
                    IdColeccion = x.IdColeccion,
                    Deleted = x.Eliminado
                })
                .ToList();

                return list;
            });
        }
    }
}
