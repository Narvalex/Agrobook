using Agrobook.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosDenormalizer : EventStreamHandler,
        IEventHandler<NuevoUsuarioCreado>
    {
        private readonly IEventStreamSubscription subscription;
        private readonly Func<UsuariosDbContext> contextFactory;
        private readonly string subName;

        public UsuariosDenormalizer(IEventStreamSubscriber subscriber, Func<UsuariosDbContext> contextFactory)
        {
            Ensure.NotNull(subscriber, nameof(subscriber));
            Ensure.NotNull(contextFactory, nameof(contextFactory));

            this.subName = this.GetType().Name;
            this.contextFactory = contextFactory;

            var lastCheckpoint = new Lazy<long?>(() =>
            {
                using (var context = this.contextFactory.Invoke())
                {
                    return context
                            .Checkpoints
                            .SingleOrDefault(c => c.Subscription == this.subName)
                            ?.LastCheckpoint;
                }
            });

            this.subscription =
                subscriber
                .CreateSubscriptionFromCategory(StreamCategoryAttribute.GetCategory<Usuario>(),
                lastCheckpoint,
                this.Dispatch);
        }

        public async Task Handle(long eventNumber, NuevoUsuarioCreado e)
        {
            await this.Denormalize(eventNumber, context =>
            {
                context.Usuarios.Add(new UsuariosEntity
                {
                    NombreDeUsuario = e.Usuario,
                    NombreCompleto = e.NombreParaMostrar,
                    AvatarUrl = e.AvatarUrl
                });
            });
        }

        public void Start()
        {
            this.subscription.Start();
        }

        public void Stop()
        {
            this.subscription.Stop();
        }

        protected override async Task Handle(long eventNumber, object @event)
        {
            await this.Denormalize(eventNumber, c => { });
        }

        private async Task Denormalize(long checkpoint, Action<UsuariosDbContext> denorm)
        {
            using (var context = this.contextFactory.Invoke())
            {
                denorm.Invoke(context);
                await context.SaveChangesAsync(this.subName, checkpoint);
            }
        }
    }
}
