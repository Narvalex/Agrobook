using Agrobook.Core;
using Agrobook.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevaOrganizacionCreada>
    {
        public OrganizacionesDenormalizer(IEventStreamSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory,
                  typeof(OrganizacionesDenormalizer).Name, 
                  StreamCategoryAttribute.GetCategory<Organizacion>().AsCategoryProjectionStream())
        { }

        public async Task Handle(long eventNumber, NuevaOrganizacionCreada e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                context.Organizaciones.Add(new OrganizacionEntity
                {
                    OrganizacionId = e.Identificador,
                    NombreParaMostrar = e.NombreParaMostrar
                });
            });
        }
    }
}
