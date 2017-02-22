using Agrobook.CLI.Utils;
using Agrobook.CLI.Views;
using Agrobook.Client.Login;
using Agrobook.Domain.Usuarios;
using System;

namespace Agrobook.CLI.Controllers
{
    public class LoginController
    {
        private readonly LoginView view;
        private readonly LoginClient loginClient;

        public LoginController(LoginView view, LoginClient tokenProvider)
        {
            this.view = view;
            this.loginClient = tokenProvider;
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

            LoginResult result;
            try
            {
                result = this.loginClient.TryLogin(userName, password).Result;
                if (result.LoginExitoso)
                    this.view.PrintLoginSuccessfully();
                else
                    this.view.PrintLoginError("Credenciales inválidas");
            }
            catch (Exception ex)
            {
                this.view.PrintLoginError(ex.GetAllMessages());
            }

            Console.ReadLine();
        }
    }
}
