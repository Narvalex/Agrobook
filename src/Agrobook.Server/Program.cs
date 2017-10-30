using System;

namespace Agrobook.Server
{
    partial class Program
    {
        static void Main(string[] args)
        {
            using (var processor = new AgrobookProcessor(args))
            {
                SetConsoleCtrlHandler(signal =>
                {
                    processor.Stop();
                    // Shutdown right away
                    Environment.Exit(-1);
                    return true;
                }, true);

                string line;
                do
                {
                    Console.WriteLine("Type exit to shut down");
                    line = Console.ReadLine();
                }
                while (!line.Equals("exit", StringComparison.InvariantCultureIgnoreCase));

                processor.Stop();
            }
        }
    }
}
