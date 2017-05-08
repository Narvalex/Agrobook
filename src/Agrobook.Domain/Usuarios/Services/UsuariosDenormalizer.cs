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
        public UsuariosDenormalizer(IEventStreamSubscriber subscriber, Func<AgrobookDbContext> contextFactory)
           : base(subscriber, contextFactory, 
                 typeof(UsuariosDenormalizer).Name, 
                 StreamCategoryAttribute.GetCategory<Usuario>().AsCategoryProjectionStream())
        { }   

        public async Task Handle(long eventNumber, NuevoUsuarioCreado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                context.Usuarios.Add(new UsuarioEntity
                {
                    NombreDeUsuario = e.Usuario,
                    NombreParaMostrar = e.NombreParaMostrar,
                    AvatarUrl = e.AvatarUrl
                });
            });
        }

        public async Task Handle(long eventNumber, AvatarUrlActualizado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(u => u.NombreDeUsuario == e.Usuario);
                usuario.AvatarUrl = e.NuevoAvatarUrl;
            });
        }

        public async Task Handle(long eventNumber, NombreParaMostrarActualizado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                var usuario = context.Usuarios.Single(u => u.NombreDeUsuario == e.Usuario);
                usuario.NombreParaMostrar = e.NuevoNombreParaMostrar;
            });
        }
    }
}
