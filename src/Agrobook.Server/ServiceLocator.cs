using Agrobook.Core;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.Cryptography;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Persistence;
using Agrobook.Infrastructure.Serialization;
using Agrobook.Infrastructure.Subscription;
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

            var sqlDbName = "AgrobookDb";

            var sqlInitializer = new SqlDbInitializer<UsuariosDbContext>(() => new UsuariosDbContext(false, sqlDbName));

            var dateTimeProvider = new SimpleDateTimeProvider();

            var decryptor = new StringCipher();

            var cryptoSerializer = new CryptoSerializer(decryptor);

            var jsonSerializer = new JsonTextSerializer();

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EventSourcedRepository(es.GetFailFastConnection, jsonSerializer, snapshotCache);

            var eventStreamSubscriber = new EventStreamSubscriber(es.ResilientConnection, jsonSerializer);

            var usuariosService = new UsuariosService(eventSourcedRepository, dateTimeProvider, cryptoSerializer);
            AutorizarAttribute.SetTokenAuthProvider(usuariosService);

            var usuariosDenormalizer = new UsuariosDenormalizer(eventStreamSubscriber, () => new UsuariosDbContext(false, sqlDbName));

            container.Register<IDateTimeProvider>(dateTimeProvider);
            container.Register<EventStoreManager>(es);
            container.Register<UsuariosService>(usuariosService);
            container.Register<SqlDbInitializer<UsuariosDbContext>>(sqlInitializer);
            container.Register<UsuariosDenormalizer>(usuariosDenormalizer);
        }
    }
}