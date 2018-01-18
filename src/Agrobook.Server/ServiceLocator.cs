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
using Agrobook.Domain.DataWarehousing;
using Agrobook.Domain.DataWarehousing.DAOs;
using Agrobook.Domain.DataWarehousing.ETLs;
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
using System.Linq;

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
            var statsPeriodSec = int.Parse(ConfigurationManager.AppSettings["statsPeriodSec"]);

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
                failFastConnectionNamePrefix: "AgrobookEventSourcedRepository",
                statsPeriodSec: statsPeriodSec);
            container.Register<EventStoreManager>(esm);


            // SQL Server ------------------------------------------------------------

            var sqlDbInitializerList = new List<ISqlDbInitializer>();

            // Relational
            var readModelConnectionString = ConfigurationManager.AppSettings["agrobookDbContext"];
            Ensure.NotNullOrWhiteSpace(readModelConnectionString, nameof(readModelConnectionString));

            Func<AgrobookDbContext> relationalDbContextFactory = () => new AgrobookDbContext(false, readModelConnectionString);
            Func<AgrobookDbContext> readOnlyRelationalDbContextFactory = () => new AgrobookDbContext(true, readModelConnectionString);
            sqlDbInitializerList.Add(new SqlDbInitializer<AgrobookDbContext>(relationalDbContextFactory));

            // DataWarehouse Db
            var dataWarehouseConnectionString = ConfigurationManager.AppSettings["agrobookDataWarehouseDbContext"];
            Ensure.NotNullOrWhiteSpace(dataWarehouseConnectionString, nameof(dataWarehouseConnectionString));

            Func<AgrobookDataWarehouseContext> dwDbContextFactory = () => new AgrobookDataWarehouseContext(false, dataWarehouseConnectionString);
            Func<AgrobookDataWarehouseContext> readOnlyDwDbContextFactory = () => new AgrobookDataWarehouseContext(true, dataWarehouseConnectionString);
            sqlDbInitializerList.Add(new SqlDbInitializer<AgrobookDataWarehouseContext>(dwDbContextFactory));

            // List of sqlDbInitializers
            container.Register<List<ISqlDbInitializer>>(sqlDbInitializerList);


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

            var usuariosQueryService = new UsuariosQueryService(readOnlyRelationalDbContextFactory, eventSourcedRepository, cryptoSerializer);
            container.Register<UsuariosQueryService>(usuariosQueryService);

            var organizacionesQueryService = new OrganizacionesQueryService(readOnlyRelationalDbContextFactory);
            container.Register<OrganizacionesQueryService>(organizacionesQueryService);

            var archivosDelProductorFileManager = new FileWriter(LogManager.GetLoggerFor<FileWriter>(), jsonSerializer);
            container.Register<IFileWriter>(archivosDelProductorFileManager);

            var archivosService = new ArchivosService(archivosDelProductorFileManager, eventSourcedRepository);
            container.Register<ArchivosService>(archivosService);

            var archivosQueryService = new ArchivosQueryService(readOnlyRelationalDbContextFactory);
            container.Register<ArchivosQueryService>(archivosQueryService);

            var apService = new ApService(eventSourcedRepository, dateTimeProvider);
            container.Register<ApService>(apService);

            var apQueryService = new ApQueryService(readOnlyRelationalDbContextFactory);
            container.Register<ApQueryService>(apQueryService);

            var apReportQueryService = new ApReportQueryService(readOnlyRelationalDbContextFactory);
            container.Register<ApReportQueryService>(apReportQueryService);

            var apDao = new ApDao(readOnlyDwDbContextFactory);
            container.Register<ApDao>(apDao);

            var numeradorDeServicios = new NumeracionDeServiciosCommandHandler(eventSourcedRepository);
            container.Register<NumeracionDeServiciosCommandHandler>(numeradorDeServicios);


            // Event Procesors -----------------------------------------------
            var orderedEventsProjDef = ProjectionDefinition.New("agrobookOrderedEvents", "agrobookOrderedEvents", esm.ProjectionManager, esm.UserCredentials)
                .From<Usuario>()
                .And<Organizacion>()
                .And<Contrato>()
                .And<Productor>()
                .And<Servicio>()
                .And<ColeccionDeArchivos>()
                .AndNothingMore();

            var procesors = new List<EventProcessor>();
            container.Register<List<EventProcessor>>(procesors);

            RegisterNumeracionProcesor(container);
            RegisterRelationalModelProcesor(container, orderedEventsProjDef, relationalDbContextFactory, readOnlyRelationalDbContextFactory);
            RegisterDataWarehouseEtlProcessor(container, orderedEventsProjDef, dwDbContextFactory, readOnlyDwDbContextFactory);
        }

        private static void RegisterNumeracionProcesor(ISimpleContainer container)
        {
            var esm = container.ResolveSingleton<EventStoreManager>();
            var jsonSerializer = container.ResolveSingleton<IJsonSerializer>();
            var eventSourcedRepository = container.ResolveSingleton<IEventSourcedRepository>();
            var apService = container.ResolveSingleton<ApService>();
            var processorList = container.ResolveSingleton<List<EventProcessor>>();

            // Numeracion de Servicios
            var numeracionDeServiciosSubscription =
                new EsSubscription(
                    StreamCategoryAttribute.GetCategoryProjectionStream<NumeracionDeServicios>(),
                    "numeracionDeServiciosEventHandler", esm.ResilientConnection, jsonSerializer);
            var numeracionDeServiciosEventHandler = new EventProcessor(numeracionDeServiciosSubscription);
            numeracionDeServiciosEventHandler.Register(new NumeracionDeServiciosEventHandler(eventSourcedRepository, apService));
            processorList.Add(numeracionDeServiciosEventHandler);
        }

        private static void RegisterRelationalModelProcesor(ISimpleContainer container, ProjectionDefinition projDef, Func<AgrobookDbContext> dbContextFactory, Func<AgrobookDbContext> readOnlyDbContextFactory)
        {
            var esm = container.ResolveSingleton<EventStoreManager>();
            var jsonSerializer = container.ResolveSingleton<IJsonSerializer>();
            var usuariosQueryService = container.ResolveSingleton<UsuariosQueryService>();
            var archivosDelProductorFileManager = container.ResolveSingleton<IFileWriter>();

            var subId = "agrobookReadModelSubscription";

            // AppReadModel Denormalizing
            var subscription = new EsSubscription(projDef, subId, esm.ResilientConnection, jsonSerializer,
                () =>
                {
                    using (var context = readOnlyDbContextFactory.Invoke())
                    {
                        return context.Checkpoints.FirstOrDefault()?.LastCheckpoint;
                    }
                });

            var sqlConfig = new AgrobookSqlDenormalizerConfig(dbContextFactory, subId);

            var processor = new EventProcessor(subscription);
            processor.Register(
                new NoOpSqlDenormalizerV1(sqlConfig),
                new UsuariosDenormalizer(sqlConfig, usuariosQueryService),
                new OrganizacionesDenormalizer(sqlConfig),
                new ContratosDenormalizer(sqlConfig),
                new ProductoresDenormalizer(sqlConfig),
                new ServiciosDenormalizer(sqlConfig),
                new ArchivosIndexer(sqlConfig, archivosDelProductorFileManager,
                    new IndizadorDeArchivosDeContratos())
            );

            container.ResolveSingleton<List<EventProcessor>>().Add(processor);
        }

        private static void RegisterDataWarehouseEtlProcessor(ISimpleContainer container, ProjectionDefinition projDef, Func<AgrobookDataWarehouseContext> dbContextFactory, Func<AgrobookDataWarehouseContext> readOnlyDbContextFactory)
        {
            var esm = container.ResolveSingleton<EventStoreManager>();
            var jsonSerializer = container.ResolveSingleton<IJsonSerializer>();
            var efCheckpointProvider = new EfCheckpointProvider<AgrobookDataWarehouseContext>(readOnlyDbContextFactory);

            var subscription = new EsSubscription(projDef, "agrobookDataWarehouseSubscription", esm.ResilientConnection, jsonSerializer,
                () => efCheckpointProvider.GetCheckpoint());

            var processor = new EventProcessor(subscription);
            processor.Register(
                new NoOpDenormalizer<AgrobookDataWarehouseContext>(dbContextFactory),
                new ApContratosEtl(dbContextFactory),
                new ApProductoresEtl(dbContextFactory),
                new ApServiciosEtl(dbContextFactory),
                new OrganizacionesEtl(dbContextFactory),
                new UsuariosEtl(dbContextFactory, container.ResolveSingleton<UsuariosQueryService>()));

            container.ResolveSingleton<List<EventProcessor>>().Add(processor);
        }
    }
}