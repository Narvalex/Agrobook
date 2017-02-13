using Agrobook.CLI.Controllers;
using Agrobook.Client.Login;

namespace Agrobook.CLI
{
    class Program
    {
        private static Controller _controller;

        static void Main(string[] args)
        {
            var accessTokenProvider = new AccessTokenProvider("http://localhost:8080");

            var loginController = new LoginController(new Views.LoginView(), accessTokenProvider);

            _controller = new Controller(new ConsoleView(), loginController);
            _controller.StartCommandLoop();
        }
    }
}
