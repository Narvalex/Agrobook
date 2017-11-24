using Agrobook.Domain.Common;
using Eventing.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public class ApReportQueryService : AgrobookQueryService
    {
        public ApReportQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader esReader) : base(contextFactory, esReader)
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
