using Agrobook.CLI.Controllers;
using System;

namespace Agrobook.CLI
{
    public class MainController
    {
        private readonly MainConsoleView view;
        private readonly LoginController loginController;
        private readonly HelpController helpController;
        private readonly SeedController seedController;

        public MainController(MainConsoleView view, LoginController loginController, HelpController helpController,
            SeedController seedController)
        {
            this.view = view;
            this.loginController = loginController;
            this.helpController = helpController;
            this.seedController = seedController;
        }

        public void StartCommandLoop()
        {
            do
            {
                this.view.Readraw();
                var cmd = Console.ReadLine();
                if (cmd.EqualsIgnoringCase("exit")) return;
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
                if (this.seedController.WasInvoked(cmd))
                {
                    this.seedController.StartSeedCommandLoop();
                    continue;
                }
            } while (true);
        }
    }
}
