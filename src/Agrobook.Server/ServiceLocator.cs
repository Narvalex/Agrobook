using Agrobook.Common;
using Agrobook.Domain;
using Agrobook.Domain.Ap.Denormalizers;
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

            var dateTimeProvider = new SimpleDateTimeProvider();

            var decryptor = new StringCipher();

            var jsonSerializer = new NewtonsoftJsonSerializer();

            var cryptoSerializer = new CryptoSerializer(decryptor, jsonSerializer);

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EventStoreEventSourcedRepository(esm.GetFailFastConnection, jsonSerializer, snapshotCache);

            var eventStreamSubscriber = new EventStoreSubscriber(esm.ResilientConnection, jsonSerializer);

            var usuariosService = new UsuariosService(eventSourcedRepository, dateTimeProvider, cryptoSerializer);
            AutorizarAttribute.SetTokenAuthProvider(usuariosService);

            var usuariosQueryService = new UsuariosQueryService(readOnlyDbContextFactory, eventSourcedRepository, cryptoSerializer);

            var usuariosDenormalizer = new UsuariosDenormalizer(eventStreamSubscriber, dbContextFactory, usuariosQueryService);

            var organizacionesDenormalizer = new OrganizacionesDenormalizer(eventStreamSubscriber, dbContextFactory);

            var organizacionesQueryService = new OrganizacionesQueryService(readOnlyDbContextFactory, eventSourcedRepository);

            var archivosDelProductorFileManager = new FileWriter(LogManager.GetLoggerFor<FileWriter>(), jsonSerializer);

            var archivosService = new ArchivosService(archivosDelProductorFileManager, eventSourcedRepository);

            var archivosQueryService = new ArchivosQueryService(readOnlyDbContextFactory, eventSourcedRepository);

            var fileIndexer = new ArchivosIndexerService(eventStreamSubscriber, dbContextFactory, archivosDelProductorFileManager);

            container.Register<IJsonSerializer>(jsonSerializer);
            container.Register<IDateTimeProvider>(dateTimeProvider);
            container.Register<ITokenAuthorizationProvider>(usuariosService);
            container.Register<SqlDbInitializer<AgrobookDbContext>>(sqlInitializer);
            container.Register<UsuariosService>(usuariosService);
            container.Register<IProveedorDeFirmaDelUsuario>(usuariosService);
            container.Register<UsuariosQueryService>(usuariosQueryService);
            container.Register<UsuariosDenormalizer>(usuariosDenormalizer);
            container.Register<OrganizacionesDenormalizer>(organizacionesDenormalizer);
            container.Register<OrganizacionesQueryService>(organizacionesQueryService);
            container.Register<IFileWriter>(archivosDelProductorFileManager);
            container.Register<ArchivosService>(archivosService);
            container.Register<ArchivosQueryService>(archivosQueryService);
            container.Register<ArchivosIndexerService>(fileIndexer);

            // Ordenado a partir de aqui
            var efCheckpointRepository = new EfCheckpointRepository(dbContextFactory, readOnlyDbContextFactory);
            var esCheckpointRepository = new EventStoreCheckpointRepository(esm.GetFailFastConnection, jsonSerializer, 3);

            var apQueryService = new ApQueryService(readOnlyDbContextFactory, eventSourcedRepository);
            container.Register<ApQueryService>(apQueryService);

            var apService = new ApService(eventSourcedRepository, eventStreamSubscriber, esCheckpointRepository, dateTimeProvider);
            container.Register<ApService>(apService);

            var contratoDenormalizer = new ContratosDenormalizer(eventStreamSubscriber, dbContextFactory);
            container.Register<ContratosDenormalizer>(contratoDenormalizer);

            var productoresDenormalizer = new ProductoresDenormalizer(eventStreamSubscriber, dbContextFactory);
            container.Register<ProductoresDenormalizer>(productoresDenormalizer);

            var serviciosDenormalizer = new ServiciosDenormalizer(eventStreamSubscriber, dbContextFactory);
            container.Register<ServiciosDenormalizer>(serviciosDenormalizer);
        }
    }
}