namespace Agrobook.Domain.Usuarios.Login
{
    public class LoginInfo
    {
        public LoginInfo(string usuario, string password, string[] claims)
        {
            this.Usuario = usuario;
            this.Password = password;
            this.Claims = claims;
        }

        public string Usuario { get; }
        public string Password { get; }
        public string[] Claims { get; }
    }
}
