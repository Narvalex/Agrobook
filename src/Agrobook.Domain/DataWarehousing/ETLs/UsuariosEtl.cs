using Agrobook.Domain.Common;
using Agrobook.Domain.DataWarehousing.Dimensions;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Eventing;
using Eventing.Core.Messaging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Domain.DataWarehousing
{
    public class UsuariosEtl : EfDenormalizer<AgrobookDataWarehouseContext>,
        IHandler<NuevoUsuarioCreado>,
        IHandler<NombreParaMostrarActualizado>,
        IHandler<PermisoOtorgadoAlUsuario>,
        IHandler<PermisoRetiradoDelUsuario>
    {
        private readonly UsuariosQueryService queryService;

        public UsuariosEtl(Func<AgrobookDataWarehouseContext> contextFactory, UsuariosQueryService queryService)
            : base(contextFactory)
        {
            Ensure.NotNull(queryService, nameof(queryService));

            this.queryService = queryService;
        }

        public async Task Handle(long checkpoint, NuevoUsuarioCreado e)
        {
            var claims = this.queryService.ObtenerClaims(e.LoginInfoEncriptado);

            this.Denormalize(checkpoint, context =>
            {
                context.UsuarioDims.Add(new UsuarioDim
                {
                    IdUsuario = e.Usuario,
                    Nombre = e.NombreParaMostrar,
                    EsAdmin = claims.Any(x => x == Roles.Admin),
                    EsGerente = claims.Any(x => x == Roles.Gerente),
                    EsTecnico = claims.Any(x => x == Roles.Tecnico),
                    EsProductor = claims.Any(x => x == Roles.Productor),
                });
            });
        }

        public async Task Handle(long checkpoint, NombreParaMostrarActualizado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.UsuarioDims.Single(u => u.IdUsuario == e.Usuario);
                usuario.Nombre = e.NuevoNombreParaMostrar;
            });
        }

        public async Task Handle(long checkpoint, PermisoOtorgadoAlUsuario e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.UsuarioDims.Single(u => u.IdUsuario == e.IdUsuario);
                this.AplicarCambioDePermiso(usuario, e.Permiso, true);
            });
        }

        public async Task Handle(long checkpoint, PermisoRetiradoDelUsuario e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.UsuarioDims.Single(u => u.IdUsuario == e.IdUsuario);
                this.AplicarCambioDePermiso(usuario, e.Permiso, true);
            });
        }

        private void AplicarCambioDePermiso(UsuarioDim usuario, string permisoACambiar, bool otorgar)
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

                // Estos los ignoramos por que no nos afectan nada
                case Roles.Invitado:
                case Permisos.AdministrarOrganizaciones:
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
