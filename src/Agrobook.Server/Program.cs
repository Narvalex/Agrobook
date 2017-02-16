using Microsoft.Owin.Hosting;
using System;
using System.Runtime.InteropServices;

namespace Agrobook.Server
{
    class Program
    {
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

        static void Main(string[] args)
        {
            // Event Store
            Console.WriteLine("Loading EventStore");
            EventStoreLoader.Load();
            Console.WriteLine("EventStore is starting");

            SetConsoleCtrlHandler(
                add: true,
                handler: signal =>
                {
                    OnExit();
                    // Shutdown right away
                    Environment.Exit(-1);
                    return true;
                });

            BeforeStartingWebServer();

            // Web Api
            var baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web server...");

            Startup.OnAppDisposing = EventStoreLoader.TearDown;
            WebApp.Start<Startup>(baseUri);
            Console.WriteLine($"Server running at {baseUri} - press Enter to quit");

            Console.ReadLine();
            EventStoreLoader.TearDown();
        }

        static void OnExit()
        {
            EventStoreLoader.TearDown();
        }

        static void BeforeStartingWebServer()
        {

        }
    }
}
