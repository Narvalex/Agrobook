using Agrobook.CLI.Controllers;
using Agrobook.Client;
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
            var http = new HttpLite("http://localhost:8081");
            var accessTokenProvider = new LoginClient(http);

            var loginController = new LoginController(new Views.LoginView(), accessTokenProvider);

            _controller = new MainController(new MainConsoleView(), loginController);
            _controller.StartCommandLoop();
        }
    }
}
