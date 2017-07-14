using Agrobook.Core;
using Agrobook.Domain.Common;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevaOrganizacionCreada>,
        IEventHandler<NuevoGrupoCreado>,
        IEventHandler<UsuarioAgregadoALaOrganizacion>,
        IEventHandler<UsuarioAgregadoAUnGrupo>,
        IEventHandler<UsuarioRemovidoDeUnGrupo>
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

        public async Task Handle(long eventNumber, NuevoGrupoCreado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                context.Grupos.Add(new GrupoEntity
                {
                    Id = e.GrupoId,
                    Display = e.GrupoDisplayName,
                    OrganizacionId = e.OrganizacionId
                });
            });
        }

        public async Task Handle(long eventNumber, UsuarioAgregadoALaOrganizacion e)
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

        public async Task Handle(long eventNumber, UsuarioAgregadoAUnGrupo e)
        {
            await this.Denormalize(eventNumber, async context =>
            {
                var grupo = await context.Grupos.SingleAsync(x => x.Id == e.GrupoId && x.OrganizacionId == e.OrganizacionId);
                context.GruposDeUsuarios.Add(new GrupoDeUsuarioEntity
                {
                    UsuarioId = e.UsuarioId,
                    OrganizacionId = e.OrganizacionId,
                    GrupoId = e.GrupoId,
                    GrupoDisplay = grupo.Display
                });
            });
        }

        public async Task Handle(long eventNumber, UsuarioRemovidoDeUnGrupo e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var grupo = new GrupoDeUsuarioEntity
                {
                    UsuarioId = e.UsuarioId,
                    OrganizacionId = e.OrganizacionId,
                    GrupoId = e.GrupoId
                };

                context.Entry(grupo).State = EntityState.Deleted;
            });
        }
    }
}
