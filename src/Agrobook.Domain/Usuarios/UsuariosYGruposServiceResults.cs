namespace Agrobook.Domain.Usuarios
{
    public class LoginResult
    {
        public LoginResult(bool loginExitoso)
        {
            this.LoginExitoso = loginExitoso;
        }
        public bool LoginExitoso { get; }
    }
}
