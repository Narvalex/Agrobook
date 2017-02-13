using Microsoft.Owin.Hosting;
using System;

namespace Agrobook.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading EventStore...");
            EventStoreLoader.Load();

            var baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web server...");

            WebApp.Start<Startup>(baseUri);
            Console.WriteLine($"Server running at {baseUri} - press Enter to quit");
            Console.ReadLine();
        }
    }
}
