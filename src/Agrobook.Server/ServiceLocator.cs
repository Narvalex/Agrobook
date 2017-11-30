using Agrobook.Common;
using Agrobook.Common.Cryptography;
using Agrobook.Common.IoC;
using Agrobook.Common.Persistence;
using Agrobook.Common.Serialization;
using Agrobook.Domain;
using Agrobook.Domain.Ap;
using Agrobook.Domain.Ap.Denormalizers;
using Agrobook.Domain.Ap.Services;
using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Server.Filters;
using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Messaging;
using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using Eventing.GetEventStore;
using Eventing.GetEventStore.Messaging;
using Eventing.GetEventStore.Persistence;
using Eventing.Log;
using System;
using System.Collections.Generic;
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


            // EventStore -----------------------------------------------------------

            var esIp = ConfigurationManager.AppSettings["esIp"];
            var esTcpPort = int.Parse(ConfigurationManager.AppSettings["esTcpPort"]);
            var esHttpPort = int.Parse(ConfigurationManager.AppSettings["esHttpPort"]);
            var esUser = ConfigurationManager.AppSettings["esUser"];
            var esPass = ConfigurationManager.AppSettings["esPass"];

            Ensure.NotNullOrWhiteSpace(esIp, nameof(esIp));
            Ensure.Positive(esTcpPort, nameof(esTcpPort));
            Ensure.NotNullOrWhiteSpace(esUser, nameof(esUser));
            Ensure.NotNullOrWhiteSpace(esPass, nameof(esPass));

            var esm = new EventStoreManager(
                extIp: esIp,
                tcpPort: esTcpPort,
                defaultUserName: esUser,
                defaultPassword: esPass,
                resilientConnectionNamePrefix: "AgrobookSubscriptions",
                failFastConnectionNamePrefix: "AgrobookEventSourcedRepository");
            container.Register<EventStoreManager>(esm);


            // SQL Server ------------------------------------------------------------

            var sqlDbName = "./SQLData/AgrobookDb";

            Func<AgrobookDbContext> dbContextFactory = () => new AgrobookDbContext(false, sqlDbName);
            Func<AgrobookDbContext> readOnlyDbContextFactory = () => new AgrobookDbContext(true, sqlDbName);

            var sqlInitializer = new SqlDbInitializer<AgrobookDbContext>(dbContextFactory);
            container.Register<SqlDbInitializer<AgrobookDbContext>>(sqlInitializer);


            // Misc --------------------------------------------------------------------

            var dateTimeProvider = new SimpleDateTimeProvider();
            container.Register<IDateTimeProvider>(dateTimeProvider);

            var decryptor = new StringCipher();

            var jsonSerializer = new NewtonsoftJsonSerializer();
            container.Register<IJsonSerializer>(jsonSerializer);

            var cryptoSerializer = new CryptoSerializer(decryptor, jsonSerializer);

            var snapshotCache = new SnapshotCache();

            var eventSourcedRepository = new EsEventSourcedRepository(esm.GetFailFastConnection, jsonSerializer, snapshotCache);
            container.Register<IEventSourcedRepository>(eventSourcedRepository);


            // Services --------------------------------------------------------

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

            var apService = new ApService(eventSourcedRepository, dateTimeProvider);
            container.Register<ApService>(apService);

            var apQueryService = new ApQueryService(readOnlyDbContextFactory, eventSourcedRepository);
            container.Register<ApQueryService>(apQueryService);

            var apReportQueryService = new ApReportQueryService(readOnlyDbContextFactory, eventSourcedRepository);
            container.Register<ApReportQueryService>(apReportQueryService);

            var numeradorDeServicios = new NumeracionDeServiciosCommandHandler(eventSourcedRepository);
            container.Register<NumeracionDeServiciosCommandHandler>(numeradorDeServicios);


            // Event Procesors -----------------------------------------------
            var procesors = new List<EventProcessor>();
            container.Register<List<EventProcessor>>(procesors);

            BuildNumeracionProcesor(container);
            BuildReadModel(container, dbContextFactory, readOnlyDbContextFactory);
        }

        private static void BuildNumeracionProcesor(ISimpleContainer container)
        {
            var esm = container.ResolveSingleton<EventStoreManager>();
            var jsonSerializer = container.ResolveSingleton<IJsonSerializer>();
            var eventSourcedRepository = container.ResolveSingleton<IEventSourcedRepository>();
            var apService = container.ResolveSingleton<ApService>();
            var processorList = container.ResolveNewOf<List<EventProcessor>>();

            // Numeracion de Servicios
            var numeracionDeServiciosSubscription =
                new EsSubscription(
                    StreamCategoryAttribute.GetCategoryProjectionStream<NumeracionDeServicios>(),
                    "numeracionDeServiciosEventHandler", esm.ResilientConnection, jsonSerializer);
            var numeracionDeServiciosEventHandler = new EventProcessor(numeracionDeServiciosSubscription);
            numeracionDeServiciosEventHandler.Register(new NumeracionDeServiciosEventHandler(eventSourcedRepository, apService));
            processorList.Add(numeracionDeServiciosEventHandler);
        }

        private static void BuildReadModel(ISimpleContainer container, Func<AgrobookDbContext> dbContextFactory, Func<AgrobookDbContext> readOnlyDbContextFactory)
        {
            var esm = container.ResolveSingleton<EventStoreManager>();
            var jsonSerializer = container.ResolveSingleton<IJsonSerializer>();
            var efCheckpointProvider = new EfCheckpointProvider(readOnlyDbContextFactory);

            //var subStream = "agrobookAppReadModelStream"; V1 projection. Not event order was guaranteed here
            var subStream = "agrobookOrderedEvents";
            var subId = "agrobookReadModelSubscription";

            var agrobookOrderedEventsProjectionDef = ProjectionDefinition.New(subStream, subStream, esm.ProjectionManager, esm.UserCredentials)
                .From<Usuario>()
                .And<Organizacion>()
                .And<Contrato>()
                .And<Productor>()
                .And<Servicio>()
                .And<ColeccionDeArchivos>()
                .AndNothingMore();

            // AppReadModel Denormalizing
            var appReadModelSubscription = new EsSubscription(agrobookOrderedEventsProjectionDef, subId, esm.ResilientConnection, jsonSerializer, () => efCheckpointProvider.GetCheckpoint(subId));

            var sqlConfig = new SqlDenormalizerConfigV1(dbContextFactory, subId);

            var appReadModelProcessor = new EventProcessor(appReadModelSubscription);
            appReadModelProcessor.Register(
                new NoOpSqlDenormalizer(sqlConfig),
                new UsuariosDenormalizer(sqlConfig, usuariosQueryService),
                new OrganizacionesDenormalizer(sqlConfig),
                new ContratosDenormalizer(sqlConfig),
                new ProductoresDenormalizer(sqlConfig),
                new ServiciosDenormalizer(sqlConfig),
                new ArchivosIndexer(sqlConfig, archivosDelProductorFileManager)
            );
            procesors.Add(appReadModelProcessor);
        }
    }
}