namespace Agrobook.Domain.Usuarios
{
    public class LoginResult
    {
        public LoginResult(bool loginExitoso, string token)
        {
            this.LoginExitoso = loginExitoso;
            this.Token = token;
        }
        public bool LoginExitoso { get; }
        public string Token { get; }
    }
}
