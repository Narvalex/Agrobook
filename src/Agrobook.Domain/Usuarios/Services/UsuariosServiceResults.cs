namespace Agrobook.Domain.Usuarios
{
    public class LoginResult
    {
        public LoginResult(bool loginExitoso, string nombreParaMostrar, string token)
        {
            this.LoginExitoso = loginExitoso;
            this.NombreParaMostrar = nombreParaMostrar;
            this.Token = token;
        }
        public bool LoginExitoso { get; }
        public string NombreParaMostrar { get; }
        public string Token { get; }
    }
}
