using System;

namespace Agrobook.Server
{
    partial class Program
    {
        static void Main(string[] args)
        {
            using (var processor = new AgrobookProcessor(args))
            {
                processor.Start();

                OnProgramExit(processor.Stop);

                string line;
                do
                {
                    Console.WriteLine("=> Type 'exit' to shut down");
                    line = Console.ReadLine();
                }
                while (!line.Equals("exit", StringComparison.InvariantCultureIgnoreCase));

                processor.Stop();
            }
        }
    }
}
