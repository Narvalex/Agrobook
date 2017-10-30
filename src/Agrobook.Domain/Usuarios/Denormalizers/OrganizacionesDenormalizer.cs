using Agrobook.Domain.Common;
using Eventing.Core.Messaging;
using System.Data.Entity;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesDenormalizer : SqlDenormalizer,
        IHandler<NuevaOrganizacionCreada>,
        IHandler<OrganizacionEliminada>,
        IHandler<OrganizacionRestaurada>,
        IHandler<UsuarioAgregadoALaOrganizacion>,
        IHandler<UsuarioRemovidoDeLaOrganizacion>
    {
        public OrganizacionesDenormalizer(SqlDenormalizerConfig config)
            : base(config)
        { }

        public void Handle(long eventNumber, NuevaOrganizacionCreada e)
        {
            this.Denormalize(eventNumber, context =>
            {
                context.Organizaciones.Add(new OrganizacionEntity
                {
                    OrganizacionId = e.Identificador,
                    NombreParaMostrar = e.NombreParaMostrar,
                    EstaEliminada = false
                });
            });
        }

        public void Handle(long eventNumber, OrganizacionEliminada e)
        {
            this.Denormalize(eventNumber, async context =>
            {
                var entity = await context.Organizaciones.SingleAsync(x => x.OrganizacionId == e.Id);
                entity.EstaEliminada = true;
            });
        }

        public void Handle(long eventNumber, OrganizacionRestaurada e)
        {
            this.Denormalize(eventNumber, async context =>
            {
                var entity = await context.Organizaciones.SingleAsync(x => x.OrganizacionId == e.Id);
                entity.EstaEliminada = false;
            });
        }

        public void Handle(long eventNumber, UsuarioAgregadoALaOrganizacion e)
        {
            this.Denormalize(eventNumber, async context =>
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

        public void Handle(long eventNumber, UsuarioRemovidoDeLaOrganizacion e)
        {
            this.Denormalize(eventNumber, async context =>
            {
                var entity = await context.OrganizacionesDeUsuarios.SingleAsync(x => x.UsuarioId == e.IdUsuario && x.OrganizacionId == e.IdOrganizacion);
                context.OrganizacionesDeUsuarios.Remove(entity);
            });
        }
    }
}
