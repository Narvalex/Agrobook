using System;

namespace Agrobook.CLI
{
    public class ConsoleView
    {
        public void Readraw()
        {
            Console.Clear();
            Console.WriteLine("Available Commands:");
            Console.WriteLine("\t login");
            Console.Write("Command:");
        }
    }
}
