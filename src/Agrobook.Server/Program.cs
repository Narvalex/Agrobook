using Agrobook.Core;
using Agrobook.Domain.Usuarios;
using Agrobook.Infrastructure.EventSourcing;
using Microsoft.Owin.Hosting;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Agrobook.Server
{
    class Program
    {
        private static ISimpleContainer container;

        #region Extern References
        // Source: http://stackoverflow.com/questions/474679/capture-console-exit-c-sharp
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(ExtConsoleHandler handler, bool add);
        private delegate bool ExtConsoleHandler(CtrlType signal);

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }
        #endregion

        static void Main(string[] args)
        {
            Console.Write("Resolving dependencies...");
            ServiceLocator.Initialize();
            container = ServiceLocator.Container;
            Console.WriteLine("Done");

            Console.Write("Initializing EventStore...");
            var es = ServiceLocator.Container.ResolveSingleton<EventStoreManager>();
            es.InitializeDb();
            Console.WriteLine("Done");

            SetConsoleCtrlHandler(
                add: true,
                handler: signal =>
                {
                    OnExit();
                    // Shutdown right away
                    Environment.Exit(-1);
                    return true;
                });

            // Web Api
            var baseUri = "http://localhost:8081";

            OnEventStoreStarted();

            Console.Write("Starting web server...");

            WebApiStartup.OnAppDisposing = () => OnExit();
            WebApp.Start<WebApiStartup>(baseUri);
            Console.WriteLine("Done");
            Console.WriteLine($"Server running at {baseUri} - press Enter to quit");

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
            container
                .ResolveSingleton<EventStoreManager>()
                .TearDown();
        }

        private static void OnEventStoreStarted()
        {
            var userService = container.ResolveSingleton<UsuariosYGruposService>();
            var intentos = 0;
            var maxRetries = 3;
            try
            {
                CrearUsuarioAdminSiHaceFalta(userService);
            }
            catch (Exception ex)
            {
                intentos++;
                Console.WriteLine("Error al verificar usuario admin");
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Intento {intentos}/{maxRetries}");
                if (intentos >= maxRetries)
                    throw;
                Thread.Sleep(1500);
                CrearUsuarioAdminSiHaceFalta(userService);
            }
        }

        private static void CrearUsuarioAdminSiHaceFalta(UsuariosYGruposService userService)
        {
            if (!userService.ExisteUsuarioAdmin)
            {
                Console.Write("Se detectó la ausencia del usuario admin. Creando uno...");
                userService.CrearUsuarioAdminAsync().Wait();
                Console.WriteLine("Listo!");
            }
        }
    }
}
