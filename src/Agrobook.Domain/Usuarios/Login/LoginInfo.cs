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
        public string Password { get; private set; }
        public string[] Claims { get; }

        public void ActualizarPassword(string nuevoPassword)
        {
            this.Password = nuevoPassword;
        }
    }
}
