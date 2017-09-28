using System;

namespace Agrobook.CLI.Views
{
    public class LoginView
    {
        public void ShowWellcomeScreen()
        {
            Console.WriteLine("Wellcome to the login screen");
        }

        public void AskForUsername()
        {
            Console.WriteLine("Enter your user name");
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void TellTextCanNotBeEmpty(string text)
        {
            Console.WriteLine($"{text} can not be empty");
        }

        public void AskForPassword()
        {
            Console.WriteLine("Enter your password");
        }

        public void PrintLoginError(string errorMessage = "")
        {
            Console.WriteLine($"Login error. {errorMessage}");
            Console.WriteLine("Press enter to continue");
        }

        public void PrintLoginSuccessfully()
        {
            Console.WriteLine("You are now sucessfully logged in. Press enter to continue.");
        }
    }
}
