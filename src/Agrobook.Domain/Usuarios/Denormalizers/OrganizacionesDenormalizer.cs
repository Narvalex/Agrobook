using Agrobook.Common;
using Agrobook.Domain.Common;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevaOrganizacionCreada>,
        IEventHandler<UsuarioAgregadoALaOrganizacion>,
        IEventHandler<UsuarioRemovidoDeLaOrganizacion>
    {
        public OrganizacionesDenormalizer(IEventSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
            : base(subscriber, contextFactory,
                  typeof(OrganizacionesDenormalizer).Name,
                  StreamCategoryAttribute.GetCategoryProjectionStream<Organizacion>())
        { }

        public async Task HandleOnce(long eventNumber, NuevaOrganizacionCreada e)
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

        public async Task HandleOnce(long eventNumber, UsuarioAgregadoALaOrganizacion e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var org = await context.Organizaciones.SingleAsync(x => x.OrganizacionId == e.OrganizacionId);
                context.OrganizacionesDeUsuarios.Add(new OrganizacionDeUsuarioEntity
                {
                    OrganizacionId = e.OrganizacionId,
                    UsuarioId = e.UsuarioId,
                    OrganizacionDisplay = org.NombreParaMostrar
                });
            });
        }

        public async Task HandleOnce(long eventNumber, UsuarioRemovidoDeLaOrganizacion e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var entity = await context.OrganizacionesDeUsuarios.SingleAsync(x => x.UsuarioId == e.IdUsuario && x.OrganizacionId == e.IdOrganizacion);
                context.OrganizacionesDeUsuarios.Remove(entity);
            });
        }
    }
}
