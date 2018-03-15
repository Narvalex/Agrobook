using Agrobook.Common.Persistence;
using Agrobook.Domain.Archivos.Services;
using Eventing;
using Eventing.Core.Messaging;
using Eventing.GetEventStore;
using Eventing.Log;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Agrobook.Server
{
    public sealed class AgrobookProcessor : IDisposable
    {
#if DEBUG
        private readonly string dropDbParam = "dropdb";
#endif
        private readonly string dropReadModelsParam = "dropreadmodels";

        private readonly bool dropDb = false;
        private readonly bool dropReadModels = false;

        private readonly ILogLite log = LogManager.GetLoggerFor<AgrobookProcessor>();

        private readonly List<EventProcessor> eventProcessors;

        public AgrobookProcessor(string[] args)
        {
            // ESTE CODIGO NO DEBERIA ESTAR POR QUE ESTA LUEGO EN EL LogManager...
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception)
                    this.log.Error((Exception)e.ExceptionObject, "Ocurrió un error no manejado");
                else
                    this.log.Error("Ocurrió un error no manejado desconocido");
            };

#if DEBUG
            this.dropDb = args.Any(x => x == this.dropDbParam);
#endif
            this.dropReadModels = args.Any(x => string.Equals(x, this.dropReadModelsParam, StringComparison.InvariantCultureIgnoreCase));

            ServiceLocator.Initialize();
            this.eventProcessors = ServiceLocator.ResolveSingleton<List<EventProcessor>>();
        }

        public void Start()
        {
            this.InitializePersistenceEngines();
            this.InitializeWebServer();
            this.WaitForEventStoreToBeReady();
            this.eventProcessors.ForEach(p => p.Start(ex =>
            {
                this.log.Fatal("Agrobook Processor will shutdown due to an unrecoverable error in an event processor.");
                this.Stop();
                Environment.Exit(1);
            }));
        }

        public void Stop()
        {
            this.eventProcessors.ForEach(p => p.Stop());
        }

        private void InitializePersistenceEngines()
        {
            // EventStore
            this.log.Verbose("Initializing EventStore...");
            var es = ServiceLocator.ResolveSingleton<EventStoreManager>();
#if DROP_DB
            this.log.Info("The database initializer configuration is DROP DB");
            es.DropAndCreateDb();
#endif
#if DEBUG
            if (this.dropDb)
            {
                this.log.Warning("The databse initializer configuration is DROP DB");
                es.DropAndCreateDb();
            }
            else
            {
                this.log.Info("The database initializer configuration is CREATE IF NOT EXISTS");
                es.CreateDbIfNotExists();
            }
#endif
#if !DEBUG
            this.log.Info("The database initializer configuration is CREATE IF NOT EXISTS");
            es.CreateDbIfNotExists();
#endif
            this.log.Verbose("Event Store is ready");

            // SQL
            var sqlInits = ServiceLocator.ResolveSingleton<List<ISqlDbInitializer>>();
#if DROP_DB
            sqlInits.ForEach(x => x.DropAndCreateDb());
#endif
#if DEBUG
            if (this.dropDb)
                sqlInits.ForEach(x => x.DropAndCreateDb());
            else
                sqlInits.ForEach(x => x.CreateDatabaseIfNotExists());
#endif
            if (!this.dropDb && this.dropReadModels)
                sqlInits.ForEach(x => x.DropAndCreateDb());

#if !DEBUG
            sqlInits.ForEach(x => x.CreateDatabaseIfNotExists());
#endif
            this.log.Verbose("SQL Compact is ready");


            var fileManager = ServiceLocator.ResolveSingleton<IFileWriter>();
#if DROP_DB
            fileManager.DeleteAllAndStartAgain();
#endif
#if DEBUG
            if (this.dropDb)
                fileManager.DeleteAllAndStartAgain();
            else
                fileManager.CreateDirectoryIfNeeded();
#endif
#if !DEBUG
            fileManager.CreateDirectoryIfNeeded();
#endif
        }

        private void InitializeWebServer()
        {
            // Web Api
            this.log.Verbose("Starting web server...");
            var serverUrl = ConfigurationManager.AppSettings["serverUrl"];
            Ensure.NotNullOrWhiteSpace(serverUrl, nameof(serverUrl));
            //WebApiStartup.OnAppDisposing = () => OnExit();
            WebApp.Start<WebApiStartup>(serverUrl);
            this.log.Success($"Web server is ready! Server running at {serverUrl}");
        }

        private void WaitForEventStoreToBeReady()
        {
            this.log.Verbose("Waiting for EventStore...");
            var esm = ServiceLocator.ResolveSingleton<EventStoreManager>();
            esm.WaitForEventStoreToBeReady().Wait();
            this.log.Success("EventStore is ready!");
        }

        public void Dispose()
        {
            ServiceLocator.ResolveSingleton<EventStoreManager>().TearDown();
            ServiceLocator.TearDown();
        }
    }
}
