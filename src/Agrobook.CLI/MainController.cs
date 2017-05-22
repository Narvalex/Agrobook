using Agrobook.CLI.Controllers;
using System;

namespace Agrobook.CLI
{
    public class MainController
    {
        private readonly MainConsoleView view;
        private readonly LoginController loginController;
        private readonly HelpController helpController;

        public MainController(MainConsoleView view, LoginController loginController, HelpController helpController)
        {
            this.view = view;
            this.loginController = loginController;
            this.helpController = helpController;
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
                if (this.helpController.WasInvoked(cmd))
                {
                    this.helpController.StartHelpCommandLoop();
                    continue;
                }
                if (this.loginController.WasInvoked(cmd))
                {
                    this.loginController.StartLoginCommandLoop();
                    continue;
                }
            } while (true);
        }
    }
}
