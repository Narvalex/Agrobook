﻿using Agrobook.Domain;
using Agrobook.Domain.Ap.Denormalizers;
using Agrobook.Domain.Ap.Services;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Infrastructure.Persistence;
using Eventing;
using Eventing.GetEventStore;
using Eventing.Log;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Agrobook.Server
{
    partial class Program
    {
#if DEBUG 
        private static string dropDbParam = "dropdb";
#endif
        private static ILogLite _log = LogManager.GlobalLogger;

        static void Main(string[] args)
        {
            SetConsoleCtrlHandler(signal =>
              {
                  OnExit();
                  // Shutdown right away
                  Environment.Exit(-1);
                  return true;
              }, true);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception)
                    _log.Error((Exception)e.ExceptionObject, "Ocurrió un error no manejado");
                else
                    _log.Error("Ocurrió un error no manejado desconocido");
            };

            // Dependency Container
            Console.WriteLine("Agrobook Server");
            _log.Verbose("Resolving dependencies...");
            ServiceLocator.Initialize();
            _log.Verbose("All dependencies were resolved successfully");

            // EventStore
            _log.Verbose("Initializing EventStore...");
            var es = ServiceLocator.ResolveSingleton<EventStoreManager>();
#if DROP_DB
            _log.Info("The database initializer configuration is DROP DB");
            es.DropAndCreateDb();
#endif
#if DEBUG
            if (args.Any(x => x == dropDbParam))
            {
                _log.Warning("The databse initializer configuration is DROP DB");
                es.DropAndCreateDb();
            }
            else
            {
                _log.Info("The database initializer configuration is CREATE IF NOT EXISTS");
                es.CreateDbIfNotExists();
            }
#endif
#if !DEBUG
            _log.Info("The database initializer configuration is CREATE IF NOT EXISTS");
            es.CreateDbIfNotExists();
#endif
            _log.Verbose("Event Store is ready");

            // SQL
            var sqlInit = ServiceLocator.ResolveSingleton<SqlDbInitializer<AgrobookDbContext>>();
#if DROP_DB
            sqlInit.DropAndCreateDb();
#endif
#if DEBUG
            if (args.Any(x => x == dropDbParam))
                sqlInit.DropAndCreateDb();
            else
                sqlInit.CreateDatabaseIfNoExists();
#endif
#if !DEBUG
            sqlInit.CreateDatabaseIfNoExists();
#endif
            _log.Verbose("SQL Compact is ready");


            var fileManager = ServiceLocator.ResolveSingleton<IFileWriter>();
#if DROP_DB
            fileManager.DeleteAllAndStartAgain();
#endif
#if DEBUG
            if (args.Any(x => x == dropDbParam))
                fileManager.DeleteAllAndStartAgain();
            else
                fileManager.CreateDirectoryIfNeeded();
#endif
#if !DEBUG
            fileManager.CreateDirectoryIfNeeded();
#endif

            // Web Api
            _log.Verbose("Starting web server...");
            var serverUrl = ConfigurationManager.AppSettings["serverUrl"];
            Ensure.NotNullOrWhiteSpace(serverUrl, nameof(serverUrl));
            WebApiStartup.OnAppDisposing = () => OnExit();
            WebApp.Start<WebApiStartup>(serverUrl);
            _log.Success($"Web server is ready! Server running at {serverUrl}");

            Prelude();

            EnterCommandLoop();
        }

        private static void EnterCommandLoop()
        {
            string line;
            do
            {
                Console.WriteLine("Type exit to shut down");
                line = Console.ReadLine();
            }
            while (!line.Equals("exit", StringComparison.InvariantCultureIgnoreCase));

            OnExit();
        }

        private static void Prelude()
        {
            // Denormalizers
            var usuariosDenormalizer = ServiceLocator.ResolveSingleton<UsuariosDenormalizer>();
            var organizacionesDenormalizer = ServiceLocator.ResolveSingleton<OrganizacionesDenormalizer>();
            var fileIndexer = ServiceLocator.ResolveSingleton<ArchivosIndexerService>();
            var contratosDenormalizer = ServiceLocator.ResolveSingleton<ContratosDenormalizer>();
            var productoresDenormalizer = ServiceLocator.ResolveSingleton<ProductoresDenormalizer>();
            var serviciosDenormalizer = ServiceLocator.ResolveSingleton<ServiciosDenormalizer>();
            var apService = ServiceLocator.ResolveSingleton<ApService>();

            usuariosDenormalizer.Start();
            organizacionesDenormalizer.Start();
            fileIndexer.Start();
            contratosDenormalizer.Start();
            productoresDenormalizer.Start();
            serviciosDenormalizer.Start();
            apService.Start();


            // Crear usuario admin si hace falta
            var userService = ServiceLocator.ResolveSingleton<UsuariosService>();
            var userQueryService = ServiceLocator.ResolveSingleton<UsuariosQueryService>();
            var intentos = 0;
            var maxRetries = 3;
            try
            {
                CrearUsuarioAdminSiHaceFalta(userQueryService, userService);
            }
            catch (Exception ex)
            {
                intentos++;
                _log.Warning("Error al verificar usuario admin");
                Console.WriteLine(ex.Message);
                _log.Warning($"Intento {intentos}/{maxRetries}");
                if (intentos >= maxRetries)
                    throw;
                Thread.Sleep(1500);
                CrearUsuarioAdminSiHaceFalta(userQueryService, userService);
            }
        }

        private static void OnExit()
        {
            // Denormalizers
            var usuariosDenormalizer = ServiceLocator.ResolveSingleton<UsuariosDenormalizer>();
            var organizacionesDenormalizer = ServiceLocator.ResolveSingleton<OrganizacionesDenormalizer>();
            var fileIndexer = ServiceLocator.ResolveSingleton<ArchivosIndexerService>();
            var contratosDenormalizer = ServiceLocator.ResolveSingleton<ContratosDenormalizer>();
            var productoresDenormalizer = ServiceLocator.ResolveSingleton<ProductoresDenormalizer>();
            var serviciosDenormalizer = ServiceLocator.ResolveSingleton<ServiciosDenormalizer>();
            var apService = ServiceLocator.ResolveSingleton<ApService>();

            usuariosDenormalizer.Stop();
            organizacionesDenormalizer.Stop();
            fileIndexer.Stop();
            contratosDenormalizer.Stop();
            productoresDenormalizer.Stop();
            serviciosDenormalizer.Stop();
            apService.Stop();

            ServiceLocator
                .ResolveSingleton<EventStoreManager>()
                .TearDown();
        }

        private static void CrearUsuarioAdminSiHaceFalta(UsuariosQueryService query, UsuariosService userService)
        {
            if (!query.ExisteUsuarioAdmin)
            {
                _log.Verbose("Se detectó la ausencia del usuario admin. Creando uno...");
                userService.CrearUsuarioAdminAsync().Wait();
                _log.Verbose("Listo!");
            }
        }
    }
}
