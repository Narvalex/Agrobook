using Agrobook.Core;
using Agrobook.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevoUsuarioCreado>,
        IEventHandler<AvatarUrlActualizado>,
        IEventHandler<NombreParaMostrarActualizado>,
        IEventHandler<PermisoOtorgadoAlUsuario>,
        IEventHandler<PermisoRetiradoDelUsuario>
    {
        private readonly UsuariosQueryService queryService;

        public UsuariosDenormalizer(IEventStreamSubscriber subscriber, Func<AgrobookDbContext> contextFactory, UsuariosQueryService queryService)
           : base(subscriber, contextFactory,
                 typeof(UsuariosDenormalizer).Name,
                 StreamCategoryAttribute.GetCategory<Usuario>().AsCategoryProjectionStream())
        {
            Ensure.NotNull(queryService, nameof(queryService));

            this.queryService = queryService;
        }

        public async Task Handle(long eventNumber, NuevoUsuarioCreado e)
        {
            var claims = this.queryService.ObtenerClaims(e.LoginInfoEncriptado);

            await this.Denormalize(eventNumber, context =>
            {
                context.Usuarios.Add(new UsuarioEntity
                {
                    Id = e.Usuario,
                    Display = e.NombreParaMostrar,
                    AvatarUrl = e.AvatarUrl,
                    EsAdmin = claims.Any(x => x == Roles.Admin),
                    EsGerente = claims.Any(x => x == Roles.Gerente),
                    EsTecnico = claims.Any(x => x == Roles.Tecnico),
                    EsProductor = claims.Any(x => x == Roles.Productor),
                    EsInvitado = claims.Any(x => x == Roles.Invitado),
                    PuedeAdministrarOrganizaciones = claims.Any(x => x == Permisos.AdministrarOrganizaciones)
                });
            });
        }

        public async Task Handle(long eventNumber, AvatarUrlActualizado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(u => u.Id == e.Usuario);
                usuario.AvatarUrl = e.NuevoAvatarUrl;
            });
        }

        public async Task Handle(long eventNumber, NombreParaMostrarActualizado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(u => u.Id == e.Usuario);
                usuario.Display = e.NuevoNombreParaMostrar;
            });
        }

        public async Task Handle(long eventNumber, PermisoOtorgadoAlUsuario e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.IdUsuario);
                this.AplicarCambioDePermiso(usuario, e.Permiso, true);
            });
        }

        public async Task Handle(long eventNumber, PermisoRetiradoDelUsuario e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.IdUsuario);
                this.AplicarCambioDePermiso(usuario, e.Permiso, false);
            });
        }

        private void AplicarCambioDePermiso(UsuarioEntity usuario, string permisoACambiar, bool otorgar)
        {
            switch (permisoACambiar)
            {
                case Roles.Admin:
                    usuario.EsAdmin = otorgar;
                    break;

                case Roles.Gerente:
                    usuario.EsGerente = otorgar;
                    break;

                case Roles.Tecnico:
                    usuario.EsTecnico = otorgar;
                    break;

                case Roles.Productor:
                    usuario.EsProductor = otorgar;
                    break;

                case Roles.Invitado:
                    usuario.EsInvitado = otorgar;
                    break;

                case Permisos.AdministrarOrganizaciones:
                    usuario.PuedeAdministrarOrganizaciones = true;
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
