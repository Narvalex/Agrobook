using Agrobook.Client.Ap;
using Agrobook.Client.Archivos;
using Agrobook.Client.Login;
using Agrobook.Client.Usuarios;
using Agrobook.Common;
using Agrobook.Common.Cryptography;
using Agrobook.Common.IoC;
using Agrobook.Common.Persistence;
using Agrobook.Common.Serialization;
using Agrobook.Domain.Usuarios;
using Eventing.Client.Http;
using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using Eventing.EntityFramework.Persistence;
using System;
using System.Collections.Generic;

namespace Agrobook.Web
{
    public static class ServiceLocator
    {
        private static ISimpleContainer _container = new SimpleContainer();

        public static T ResolveSingleton<T>() => _container.ResolveSingleton<T>();

        public static T ResolveNewOf<T>() => _container.ResolveNewOf<T>();

        private static object localLock = new object();
        private static bool localIsRuning = false;

        public static void Initialize()
        {
            var container = _container;

            // Config
            var serverUrl = "http://localhost:8081";

            var http = new HttpLite(serverUrl);

            container.Register<ILoginClient>(() => new LoginClient(http));
            container.Register<IUsuariosClient>(() => new UsuariosClient(http));
            container.Register<IUsuariosQueryClient>(() => new UsuariosQueryClient(http));
            container.Register<IArchivosClient>(() => new ArchivosClient(http));
            container.Register<IArchivosQueryClient>(() => new ArchivosQueryClient(http));
            container.Register<IApQueryClient>(() => new ApQueryClient(http));
            container.Register<IApClient>(() => new ApClient(http));
            container.Register<IApReportClient>(() => new ApReportClient(http));
        }

        public static void InitializeLocal()
        {
            lock (localLock)
            {
                if (localIsRuning) return;
                DoInitializeLocal();
                StartLocalProcessor();
                localIsRuning = true;
            }
        }

        private static void DoInitializeLocal()
        {
            var container = _container;

            // Common -----------------------------------------------------------------------------
            var serializer = new NewtonsoftJsonSerializer();
            var cryptoSerializer = new CryptoSerializer(new StringCipher(), serializer);
            var dateTimeProvider = new SimpleDateTimeProvider();

            var snapshotCache = new SnapshotCache();
            var connectionString = "Data Source=(LocalDb)\\MSSQLLocalDb;Initial Catalog=agrobookLocalEventStore;Integrated Security=SSPI";
            Func<EventStoreDbContext> writeEventStoreDbContextFactory = () => new EventStoreDbContext(false, connectionString);
            var eventStore = new EfEventStore(snapshotCache, serializer, () => new EventStoreDbContext(true, connectionString), writeEventStoreDbContextFactory);

            // SQl Initializers --------------------------------------------------------------------
            var sqlDbInitializerList = new List<ISqlDbInitializer>();
            sqlDbInitializerList.Add(new SqlDbInitializer<EventStoreDbContext>(writeEventStoreDbContextFactory));
            // List of sqlDbInitializers
            container.Register<List<ISqlDbInitializer>>(sqlDbInitializerList);

            // Services ---------------------------------------------------------------------------
            var usuariosService = new UsuariosService(eventStore, dateTimeProvider, cryptoSerializer);

            container.Register<ILoginClient>(() => new LoginLocalClient(new SimpleDateTimeProvider(), usuariosService));
        }

        private static void StartLocalProcessor()
        {
            var sqlInits = ServiceLocator.ResolveSingleton<List<ISqlDbInitializer>>();
            sqlInits.ForEach(x => x.CreateDatabaseIfNotExists());
        }
    }
}