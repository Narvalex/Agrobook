using Agrobook.Core;
using Agrobook.Domain.Common;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosDenormalizer : AgrobookDenormalizer,
        IEventHandler<NuevoUsuarioCreado>,
        IEventHandler<AvatarUrlActualizado>,
        IEventHandler<NombreParaMostrarActualizado>
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
            var esProductor = this.queryService.EsProductor(e.LoginInfoEncriptado);

            await this.Denormalize(eventNumber, context =>
            {
                context.Usuarios.Add(new UsuarioEntity
                {
                    Id = e.Usuario,
                    Display = e.NombreParaMostrar,
                    AvatarUrl = e.AvatarUrl,
                    EsProductor = esProductor
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
    }
}
