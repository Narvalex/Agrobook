using Agrobook.CLI.Views;
using Agrobook.Client.Login;
using System;

namespace Agrobook.CLI.Controllers
{
    public class LoginController
    {
        private readonly LoginView view;
        private readonly AccessTokenProvider tokenProvider;

        public LoginController(LoginView view, AccessTokenProvider tokenProvider)
        {
            this.view = view;
            this.tokenProvider = tokenProvider;
        }

        public void StartLoginCommandLoop()
        {
            this.view.ShowWellcomeScreen();

            string userName;
            do
            {
                this.view.AskForUsername();
                userName = Console.ReadLine();
                this.view.Clear();
                if (string.IsNullOrWhiteSpace(userName))
                    this.view.TellTextCanNotBeEmpty("user name");

                break;
            } while (true);

            string password;
            do
            {
                this.view.AskForPassword();
                password = Console.ReadLine();
                this.view.Clear();
                if (string.IsNullOrWhiteSpace(password))
                    this.view.TellTextCanNotBeEmpty("password");

                break;
            } while (true);

            var tokenDictionary = this.tokenProvider.TryGetTokenDictionary(userName, password).Result;
            foreach (var item in tokenDictionary)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }
    }
}
