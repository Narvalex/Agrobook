using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesQueryService : AgrobookQueryService
    {
        public OrganizacionesQueryService(Func<AgrobookDbContext> contextFactory) : base(contextFactory)
        { }
    }
}
