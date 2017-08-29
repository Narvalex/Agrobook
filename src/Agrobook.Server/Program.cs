using Agrobook.Domain;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Services;
using Agrobook.Infrastructure.Persistence;
using Eventing.GetEventStore;
using Eventing.Log;
using Microsoft.Owin.Hosting;
using System;
using System.Threading;

namespace Agrobook.Server
{
    partial class Program
    {
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

            // Dependency Container
            Console.WriteLine("Agrobook Server");
            _log.Verbose("Resolving dependencies...");
            ServiceLocator.Initialize();
            _log.Verbose("All dependencies were resolved successfully");

            // EventStore
            _log.Verbose("Initializing EventStore...");
            var es = ServiceLocator.ResolveSingleton<EventStoreManager>();
#if DROP_DB
            _log.Info("The database initializer configuration is DROP AND CREATE");
            es.DropAndCreateDb();
#else
            _log.Info("The database initializer configuration is CREATE IF NOT EXISTS");
            es.CreateDbIfNotExists();
#endif
            _log.Verbose("Event Store is ready");

            // SQL
            var sqlInit = ServiceLocator.ResolveSingleton<SqlDbInitializer<AgrobookDbContext>>();
#if DROP_DB
            sqlInit.DropAndCreateDb();
#endif
#if !DROP_DB
            sqlInit.CreateDatabaseIfNoExists();
#endif
            _log.Verbose("SQL Compact is ready");


            var fileManager = ServiceLocator.ResolveSingleton<IArchivosDelProductorFileManager>();
#if DROP_DB
            fileManager.BorrarTodoYEmpezarDeNuevo();
#endif
#if !DROP_DB
            fileManager.CrearDirectoriosSiFaltan();
#endif

            // Web Api
            _log.Verbose("Starting web server...");
            var baseUri = "http://localhost:8081";
            WebApiStartup.OnAppDisposing = () => OnExit();
            WebApp.Start<WebApiStartup>(baseUri);
            _log.Success($"Web server is ready! Server running at {baseUri}");

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

        private static void OnExit()
        {
            ServiceLocator
                .ResolveSingleton<EventStoreManager>()
                .TearDown();
        }

        private static void Prelude()
        {
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

            var usuariosDenormalizer = ServiceLocator.ResolveSingleton<UsuariosDenormalizer>();
            var organizacionesDenormalizer = ServiceLocator.ResolveSingleton<OrganizacionesDenormalizer>();
            var fileIndexer = ServiceLocator.ResolveSingleton<ArchivosIndexerService>();

            usuariosDenormalizer.Start();
            organizacionesDenormalizer.Start();
            fileIndexer.Start();
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
