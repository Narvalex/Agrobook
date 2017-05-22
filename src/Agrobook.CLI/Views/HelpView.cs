using System;

namespace Agrobook.CLI.Views
{
    public class HelpView
    {
        public void ShowAvailableCommands(string[] commands)
        {
            Console.WriteLine("Available commands:");
            for (int i = 0; i < commands.Length; i++)
            {
                Console.WriteLine(commands[i]);
            }
        }
    }
}
