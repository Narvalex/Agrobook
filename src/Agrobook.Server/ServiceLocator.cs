using Agrobook.Common;
using Agrobook.Domain;
using Agrobook.Domain.Ap.Services;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.Cryptography;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Persistence;
using Agrobook.Infrastructure.Serialization;
using Agrobook.Server.Filters;
using Eventing;
using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using Eventing.GetEventStore;
using Eventing.GetEventStore.Persistence;
using Eventing.Log;
using System;
using System.Configuration;

namespace Agrobook.Server
{
    public static class ServiceLocator
    {
        private static ISimpleContainer _container;

        public static T ResolveSingleton<T>() => _container.ResolveSingleton<T>();

        public static T ResolveNewOf<T>() => _container.ResolveNewOf<T>();

        public static void TearDown() => _container.Dispose();

        public static void Initialize()
        {
            var container = new SimpleContainer();
            _container = container;

            var esIp = ConfigurationManager.AppSettings["esIp"];
            var esTcpPort = int.Parse(ConfigurationManager.AppSettings["esTcpPort"]);
            var esUser = ConfigurationManager.AppSettings["esUser"];
            var esPass = ConfigurationManager.AppSettings["esPass"];

            Ensure.NotNullOrWhiteSpace(esIp, nameof(esIp));
            Ensure.Positive(esTcpPort, nameof(esTcpPort));
            Ensure.NotNullOrWhiteSpace(esUser, nameof(esUser));
            Ensure.NotNullOrWhiteSpace(esPass, nameof(esPass));

            // Event Store
            var esm = new EventStoreManager(
                extIp: esIp,
                tcpPort: esTcpPort,
                defaultUserName: esUser,
                defaultPassword: esPass,
                resilientConnectionNamePrefix: "AgrobookSubscriptions",
                failFastConnectionNamePrefix: "AgrobookEventSourcedRepository");
            container.Register<EventStoreManager>(esm);

            var sqlDbName = "AgrobookDb";

            Func<AgrobookDbContext> dbContextFactory = () => new AgrobookDbContext(false, sqlDbName);
            Func<AgrobookDbContext> readOnlyDbContextFactory = () => new AgrobookDbContext(true, sqlDbName);

            var sqlInitializer = new SqlDbInitializer<AgrobookDbContext>(dbContextFactory);
            container.Register<SqlDbInitializer<AgrobookDbContext>>(sqlInitializer);

            var dateTimeProvider = new SimpleDateTimeProvider();
            container.Register<IDateTimeProvider>(dateTimeProvider);

            var decryptor = new StringCipher();

            var jsonSerializer = new NewtonsoftJsonSerializer();
            container.Register<IJsonSerializer>(jsonSerializer);

            var cryptoSerializer = new CryptoSerializer(decryptor, jsonSerializer);

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EventStoreEventSourcedRepository(esm.GetFailFastConnection, jsonSerializer, snapshotCache);

            var efCheckpointRepository = new EfCheckpointProvider(readOnlyDbContextFactory);

            // Services
            var usuariosService = new UsuariosService(eventSourcedRepository, dateTimeProvider, cryptoSerializer);
            AutorizarAttribute.SetTokenAuthProvider(usuariosService);
            container.Register<UsuariosService>(usuariosService);
            container.Register<ITokenAuthorizationProvider>(usuariosService);
            container.Register<IProveedorDeFirmaDelUsuario>(usuariosService);

            var usuariosQueryService = new UsuariosQueryService(readOnlyDbContextFactory, eventSourcedRepository, cryptoSerializer);
            container.Register<UsuariosQueryService>(usuariosQueryService);

            var organizacionesQueryService = new OrganizacionesQueryService(readOnlyDbContextFactory, eventSourcedRepository);
            container.Register<OrganizacionesQueryService>(organizacionesQueryService);

            var archivosDelProductorFileManager = new FileWriter(LogManager.GetLoggerFor<FileWriter>(), jsonSerializer);
            container.Register<IFileWriter>(archivosDelProductorFileManager);

            var archivosService = new ArchivosService(archivosDelProductorFileManager, eventSourcedRepository);
            container.Register<ArchivosService>(archivosService);

            var archivosQueryService = new ArchivosQueryService(readOnlyDbContextFactory, eventSourcedRepository);
            container.Register<ArchivosQueryService>(archivosQueryService);

            var apQueryService = new ApQueryService(readOnlyDbContextFactory, eventSourcedRepository);
            container.Register<ApQueryService>(apQueryService);

            var apService = new ApService(eventSourcedRepository, eventStreamSubscriber, esCheckpointRepository, dateTimeProvider);
            container.Register<ApService>(apService);

            // Procesors
            var usuariosDenormalizer = new UsuariosDenormalizer(eventStreamSubscriber, dbContextFactory, usuariosQueryService);

            var organizacionesDenormalizer = new OrganizacionesDenormalizer(eventStreamSubscriber, dbContextFactory);

            var fileIndexer = new ArchivosIndexer(eventStreamSubscriber, dbContextFactory, archivosDelProductorFileManager);

            var contratoDenormalizer = new ContratosDenormalizer(eventStreamSubscriber, dbContextFactory);
            container.Register<ContratosDenormalizer>(contratoDenormalizer);

            var productoresDenormalizer = new ProductoresDenormalizer(eventStreamSubscriber, dbContextFactory);
            container.Register<ProductoresDenormalizer>(productoresDenormalizer);

            var serviciosDenormalizer = new ServiciosDenormalizer(eventStreamSubscriber, dbContextFactory);
            container.Register<ServiciosDenormalizer>(serviciosDenormalizer);
        }
    }
}