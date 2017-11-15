using Agrobook.Domain.Common;
using Eventing;
using Eventing.Core.Messaging;
using System;
using System.Linq;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosDenormalizer : SqlDenormalizer,
        IHandler<NuevoUsuarioCreado>,
        IHandler<AvatarUrlActualizado>,
        IHandler<NombreParaMostrarActualizado>,
        IHandler<PermisoOtorgadoAlUsuario>,
        IHandler<PermisoRetiradoDelUsuario>,
        // telefono
        IHandler<TelefonoDeUsuarioRegistrado>,
        IHandler<TelefonoDeUsuarioActualizado>,
        IHandler<TelefonoDeUsuarioEliminado>,
        // Email
        IHandler<EmailDeUsuarioRegistrado>,
        IHandler<EmailDeUsuarioActualizado>,
        IHandler<EmailDeUsuarioEliminado>
    {
        private readonly UsuariosQueryService queryService;

        public UsuariosDenormalizer(SqlDenormalizerConfig config, UsuariosQueryService queryService)
           : base(config)
        {
            Ensure.NotNull(queryService, nameof(queryService));

            this.queryService = queryService;
        }

        public void Handle(long eventNumber, NuevoUsuarioCreado e)
        {
            var claims = this.queryService.ObtenerClaims(e.LoginInfoEncriptado);

            this.Denormalize(eventNumber, context =>
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

        public void Handle(long eventNumber, AvatarUrlActualizado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(u => u.Id == e.Usuario);
                usuario.AvatarUrl = e.NuevoAvatarUrl;
            });
        }

        public void Handle(long eventNumber, NombreParaMostrarActualizado e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(u => u.Id == e.Usuario);
                usuario.Display = e.NuevoNombreParaMostrar;
            });
        }

        public void Handle(long eventNumber, PermisoOtorgadoAlUsuario e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.IdUsuario);
                this.AplicarCambioDePermiso(usuario, e.Permiso, true);
            });
        }

        public void Handle(long eventNumber, PermisoRetiradoDelUsuario e)
        {
            this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.IdUsuario);
                this.AplicarCambioDePermiso(usuario, e.Permiso, false);
            });
        }

        public void Handle(long checkpoint, TelefonoDeUsuarioRegistrado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.StreamId);
                usuario.Telefono = e.Telefono;
            });
        }

        public void Handle(long checkpoint, TelefonoDeUsuarioActualizado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.StreamId);
                usuario.Telefono = e.Telefono;
            });
        }

        public void Handle(long checkpoint, TelefonoDeUsuarioEliminado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.StreamId);
                usuario.Telefono = null;
            });
        }

        public void Handle(long checkpoint, EmailDeUsuarioRegistrado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.StreamId);
                usuario.Email = e.Email;
            });
        }

        public void Handle(long checkpoint, EmailDeUsuarioActualizado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.StreamId);
                usuario.Email = e.Email;
            });
        }

        public void Handle(long checkpoint, EmailDeUsuarioEliminado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var usuario = context.Usuarios.Single(x => x.Id == e.StreamId);
                usuario.Email = e.Email;
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
