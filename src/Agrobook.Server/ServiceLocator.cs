using Agrobook.Core;
using Agrobook.Domain.Usuarios;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.Cryptography;
using Agrobook.Infrastructure.EventSourcing;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Serialization;
using Agrobook.Server.Filters;

namespace Agrobook.Server
{
    public static class ServiceLocator
    {
        private static ISimpleContainer _container = new SimpleContainer();

        public static T ResolveSingleton<T>() => _container.ResolveSingleton<T>();

        public static T ResolveNewOf<T>() => _container.ResolveNewOf<T>();

        public static void Initialize()
        {
            var container = _container;

            var es = new EventStoreManager();

            var dateTimeProvider = new SimpleDateTimeProvider();

            var decryptor = new StringCipher();

            var cryptoSerializer = new CryptoSerializer(decryptor);

            var jsonSerializer = new JsonTextSerializer();

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EventSourcedRepository(es.GetFailFastConnection, jsonSerializer, snapshotCache);

            var usuariosService = new UsuariosYGruposService(eventSourcedRepository, dateTimeProvider, cryptoSerializer);
            AutorizarAttribute.SetTokenAuthProvider(usuariosService);

            container.Register<IDateTimeProvider>(dateTimeProvider);
            container.Register<EventStoreManager>(es);
            container.Register<UsuariosYGruposService>(usuariosService);
        }
    }
}