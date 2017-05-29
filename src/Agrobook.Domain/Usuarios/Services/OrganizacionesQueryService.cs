using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesQueryService : AgrobookQueryService
    {
        public OrganizacionesQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader esReader) : base(contextFactory, esReader)
        { }
    }
}
