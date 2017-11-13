using Agrobook.CLI.Controllers;
using Agrobook.CLI.Views;
using Agrobook.Client.Ap;
using Agrobook.Client.Login;
using Agrobook.Client.Usuarios;
using Eventing.Client.Http;
using Eventing.Core.Serialization;

namespace Agrobook.CLI
{
    class Program
    {
        private static MainController _controller;

        static void Main(string[] args)
        {
            var serializer = new NewtonsoftJsonSerializer();
            var http = new HttpLite("http://localhost:8081");

            var accessTokenProvider = new LoginClient(http);
            var loginController = new LoginController(new LoginView(), accessTokenProvider);

            var usuariosClient = new UsuariosClient(http, () => loginController.Token);
            var apClient = new ApClient(http, () => loginController.Token);

            var seedController = new SeedController(new SeedView(), usuariosClient, apClient);

            var helpController = new HelpController(new HelpView(), loginController, seedController);

            _controller = new MainController(new MainConsoleView(), loginController, helpController, seedController);
            _controller.StartCommandLoop();
        }
    }
}
