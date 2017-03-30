using Agrobook.Core;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosDenormalizer
    {
        private readonly IEventStreamSubscription subscription;

        public UsuariosDenormalizer(IEventStreamSubscriber subscriber)
        {
            this.subscription = subscriber.CreateSubscription(null, null, null);
            this.subscription.Start();
        }

        
    }
}
