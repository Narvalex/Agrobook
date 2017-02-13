using Agrobook.CLI.Controllers;
using System;

namespace Agrobook.CLI
{
    public class Controller
    {
        private readonly ConsoleView view;
        private readonly LoginController loginController;

        public Controller(ConsoleView view, LoginController loginController)
        {
            this.view = view;
            this.loginController = loginController;
        }

        public void StartCommandLoop()
        {
            this.view.Readraw();
            do
            {
                var cmd = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cmd))
                {
                    this.view.Readraw();
                    continue;
                }
                // Single token commands
                if (cmd.Equals("login", StringComparison.OrdinalIgnoreCase))
                {
                    this.loginController.StartLoginCommandLoop();
                    continue;
                }
            } while (true);
        }
    }
}
