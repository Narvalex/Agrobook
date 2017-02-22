using Agrobook.CLI.Controllers;
using System;

namespace Agrobook.CLI
{
    public class MainController
    {
        private readonly MainConsoleView view;
        private readonly LoginController loginController;

        public MainController(MainConsoleView view, LoginController loginController)
        {
            this.view = view;
            this.loginController = loginController;
        }

        public void StartCommandLoop()
        {
            do
            {
                this.view.Readraw();
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
