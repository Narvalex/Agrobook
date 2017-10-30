using Agrobook.Domain;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Usuarios;
using Agrobook.Infrastructure.Persistence;
using Eventing;
using Eventing.Core.Messaging;
using Eventing.GetEventStore;
using Eventing.Log;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Agrobook.Server
{
    public sealed class AgrobookProcessor : IDisposable
    {
#if DEBUG
        private readonly string dropDbParam = "dropdb";
        private readonly bool dropDb = false;
#endif
        private readonly ILogLite log = LogManager.GetLoggerFor<AgrobookProcessor>();

        private readonly List<EventProcessor> processors;

        public AgrobookProcessor(string[] args)
        {
#if DEBUG
            this.dropDb = args.Any(x => x == this.dropDbParam);
#endif
            ServiceLocator.Initialize();
            this.processors = ServiceLocator.ResolveSingleton<List<EventProcessor>>();
        }

        public void Start()
        {
            this.InitializePersistenceEngines();
            this.InitializeWebServer();
            this.processors.ForEach(p => p.Start());
            this.CheckDefaultAdminUser();
        }

        public void Stop()
        {
            this.processors.ForEach(p => p.Stop());
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
            var sqlInit = ServiceLocator.ResolveSingleton<SqlDbInitializer<AgrobookDbContext>>();
#if DROP_DB
            sqlInit.DropAndCreateDb();
#endif
#if DEBUG
            if (this.dropDb)
                sqlInit.DropAndCreateDb();
            else
                sqlInit.CreateDatabaseIfNoExists();
#endif
#if !DEBUG
            sqlInit.CreateDatabaseIfNoExists();
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

        private void CheckDefaultAdminUser()
        {
            var tryCount = 0;
            var maxRetries = 10;
            var usuariosService = ServiceLocator.ResolveSingleton<UsuariosService>();
            var sigaIntentando = true;
            do
            {
                try
                {
                    usuariosService.CrearUsuarioAdminSiFaltaAsync().Wait();
                    sigaIntentando = false;
                }
                catch (Exception ex)
                {
                    tryCount++;
                    this.log.Warning($"Error al verificar usuario admin. {ex.Message}. Intento {tryCount} de {maxRetries}");
                    if (tryCount >= maxRetries)
                        throw;
                    Thread.Sleep(1000 * tryCount);
                }
            } while (sigaIntentando);
        }

        private void SetGlobalExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception)
                    this.log.Error((Exception)e.ExceptionObject, "Ocurrió un error no manejado");
                else
                    this.log.Error("Ocurrió un error no manejado desconocido");
            };
        }

        public void Dispose()
        {
            ServiceLocator.ResolveSingleton<EventStoreManager>()
                .TearDown();
            ServiceLocator.TearDown();
        }
    }
}
