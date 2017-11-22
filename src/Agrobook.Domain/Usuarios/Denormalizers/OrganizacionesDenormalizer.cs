using Agrobook.Domain.Common;
using Eventing.Core.Messaging;
using System.Linq;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesDenormalizer : SqlDenormalizer,
        IHandler<NuevaOrganizacionCreada>,
        IHandler<NombreDeOrganizacionCambiado>,
        IHandler<OrganizacionEliminada>,
        IHandler<OrganizacionRestaurada>,
        IHandler<UsuarioAgregadoALaOrganizacion>,
        IHandler<UsuarioRemovidoDeLaOrganizacion>
    {
        public OrganizacionesDenormalizer(SqlDenormalizerConfig config)
            : base(config)
        { }

        public void Handle(long checkpoint, NuevaOrganizacionCreada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                context.Organizaciones.Add(new OrganizacionEntity
                {
                    OrganizacionId = e.Identificador,
                    NombreParaMostrar = e.NombreParaMostrar,
                    EstaEliminada = false
                });
            });
        }

        public void Handle(long checkpoint, NombreDeOrganizacionCambiado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var entity = context.Organizaciones.Single(x => x.OrganizacionId == e.IdOrg);
                entity.NombreParaMostrar = e.NombreNuevo;
            });
        }

        public void Handle(long checkpoint, OrganizacionEliminada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var entity = context.Organizaciones.Single(x => x.OrganizacionId == e.Id);
                entity.EstaEliminada = true;
            });
        }

        public void Handle(long checkpoint, OrganizacionRestaurada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var entity = context.Organizaciones.Single(x => x.OrganizacionId == e.Id);
                entity.EstaEliminada = false;
            });
        }

        public void Handle(long checkpoint, UsuarioAgregadoALaOrganizacion e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var org = context.Organizaciones.Single(x => x.OrganizacionId == e.OrganizacionId);
                context.OrganizacionesDeUsuarios.Add(new OrganizacionDeUsuarioEntity
                {
                    OrganizacionId = e.OrganizacionId,
                    UsuarioId = e.UsuarioId,
                    OrganizacionDisplay = org.NombreParaMostrar
                });
            });
        }

        public void Handle(long checkpoint, UsuarioRemovidoDeLaOrganizacion e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var entity = context.OrganizacionesDeUsuarios.Single(x => x.UsuarioId == e.IdUsuario && x.OrganizacionId == e.IdOrganizacion);
                context.OrganizacionesDeUsuarios.Remove(entity);
            });
        }
    }
}
