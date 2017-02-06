using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var baseUri = "http://localhost:8080";

            Console.WriteLine("Starting web server...");

            WebApp.Start<Startup>(baseUri);
            Console.WriteLine($"Server running at {baseUri} - press Enter to quit");
            Console.ReadLine();
        }
    }
}
