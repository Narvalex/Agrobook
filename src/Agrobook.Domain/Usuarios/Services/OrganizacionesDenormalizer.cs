using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesDenormalizer : AgrobookDenormalizer
    {
        public OrganizacionesDenormalizer(IEventStreamSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory,
                  typeof(OrganizacionesDenormalizer).Name, 
                  StreamCategoryAttribute.GetCategory<Organizacion>().AsCategoryProjectionStream())
        { }


    }
}
