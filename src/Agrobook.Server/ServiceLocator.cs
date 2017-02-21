using Agrobook.Core;
using Agrobook.Domain.Usuarios;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.EventSourcing;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Serialization;

namespace Agrobook.Server
{
    public static class ServiceLocator
    {
        public static ISimpleContainer Container { get; } = new SimpleContainer();

        public static void Initialize()
        {
            var container = Container;

            var es = new EventStoreManager();

            var dateTimeProvider = new SimpleDateTimeProvider();

            var jsonSerializer = new JsonSerializer();

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EventSourcedRepository(es.GetFailFastConnection, jsonSerializer, snapshotCache);

            var usuariosService = new UsuariosYGruposService(eventSourcedRepository, dateTimeProvider);

            container.Register<EventStoreManager>(es);
            container.Register<UsuariosYGruposService>(usuariosService);
        }
    }
}