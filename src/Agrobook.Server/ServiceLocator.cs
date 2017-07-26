using Agrobook.Core;
using Agrobook.Domain;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.Cryptography;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Log;
using Agrobook.Infrastructure.Persistence;
using Agrobook.Infrastructure.Serialization;
using Agrobook.Infrastructure.Subscription;
using Agrobook.Server.Filters;
using System;

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

            var esm = new EventStoreManager();

            var sqlDbName = "AgrobookDb";

            Func<AgrobookDbContext> dbContextFactory = () => new AgrobookDbContext(false, sqlDbName);
            Func<AgrobookDbContext> readOnlyDbContextFactory = () => new AgrobookDbContext(true, sqlDbName);

            var sqlInitializer = new SqlDbInitializer<AgrobookDbContext>(dbContextFactory);

            var dateTimeProvider = new SimpleDateTimeProvider();

            var decryptor = new StringCipher();

            var cryptoSerializer = new CryptoSerializer(decryptor);

            var jsonSerializer = new JsonTextSerializer();

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EventSourcedRepository(esm.GetFailFastConnection, jsonSerializer, snapshotCache);

            var eventStreamSubscriber = new EventStreamSubscriber(esm.ResilientConnection, jsonSerializer);

            var usuariosService = new UsuariosService(eventSourcedRepository, dateTimeProvider, cryptoSerializer);
            AutorizarAttribute.SetTokenAuthProvider(usuariosService);

            var usuariosQueryService = new UsuariosQueryService(readOnlyDbContextFactory, eventSourcedRepository, cryptoSerializer);

            var usuariosDenormalizer = new UsuariosDenormalizer(eventStreamSubscriber, dbContextFactory, usuariosQueryService);

            var organizacionesDenormalizer = new OrganizacionesDenormalizer(eventStreamSubscriber, dbContextFactory);

            var organizacionesQueryService = new OrganizacionesQueryService(readOnlyDbContextFactory, eventSourcedRepository);

            var archivosDelProductorFileManager = new DurableArchivosDelProductorFileManager(LogManager.GetLoggerFor<DurableArchivosDelProductorFileManager>(), jsonSerializer);

            var archivosService = new ArchivosService(archivosDelProductorFileManager, eventSourcedRepository, dateTimeProvider);

            var archivosQueryService = new ArchivosQueryService(readOnlyDbContextFactory, eventSourcedRepository);

            var fileIndexer = new ArchivosIndexerService(eventStreamSubscriber, dbContextFactory, archivosDelProductorFileManager);

            container.Register<IJsonSerializer>(jsonSerializer);
            container.Register<IDateTimeProvider>(dateTimeProvider);
            container.Register<ITokenAuthorizationProvider>(usuariosService);
            container.Register<EventStoreManager>(esm);
            container.Register<SqlDbInitializer<AgrobookDbContext>>(sqlInitializer);
            container.Register<UsuariosService>(usuariosService);
            container.Register<IProveedorDeMetadatosDelUsuario>(usuariosService);
            container.Register<UsuariosQueryService>(usuariosQueryService);
            container.Register<UsuariosDenormalizer>(usuariosDenormalizer);
            container.Register<OrganizacionesDenormalizer>(organizacionesDenormalizer);
            container.Register<OrganizacionesQueryService>(organizacionesQueryService);
            container.Register<IArchivosDelProductorFileManager>(archivosDelProductorFileManager);
            container.Register<ArchivosService>(archivosService);
            container.Register<ArchivosQueryService>(archivosQueryService);
            container.Register<ArchivosIndexerService>(fileIndexer);
        }
    }
}