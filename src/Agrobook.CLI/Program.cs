using Agrobook.CLI.Controllers;
using Agrobook.Client.Login;
using Agrobook.Infrastructure.Serialization;

namespace Agrobook.CLI
{
    class Program
    {
        private static MainController _controller;

        static void Main(string[] args)
        {
            var serializer = new JsonTextSerializer();
            var accessTokenProvider = new LoginClient("http://localhost:8080", serializer);

            var loginController = new LoginController(new Views.LoginView(), accessTokenProvider);

            _controller = new MainController(new MainConsoleView(), loginController);
            _controller.StartCommandLoop();
        }
    }
}
