using Agrobook.Domain.Common;
using Eventing.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public class ApReportQueryService : DbContextQueryService<AgrobookDbContext>
    {
        public ApReportQueryService(Func<AgrobookDbContext> contextFactory) : base(contextFactory)
        { }

        public async Task<IList<ProductorRp>> ObtenerTodosProductores()
            => await this.QueryAsync(async context =>
            await context
            .Usuarios
            .Where(u => u.EsProductor)
            .Select(u => new ProductorRp
            {
                ProductorId = u.Id,
                ProductorDisplay = u.Display
            })
            .ToListAsync());
    }
}
