namespace Agrobook.Domain.Usuarios
{
    public class LoginResult
    {
        public LoginResult(bool loginExitoso, string usuario, string nombreParaMostrar, string token, string avatarUrl)
        {
            this.LoginExitoso = loginExitoso;
            this.Usuario = usuario;
            this.NombreParaMostrar = nombreParaMostrar;
            this.Token = token;
            this.AvatarUrl = avatarUrl;
        }
        public bool LoginExitoso { get; }
        public string Usuario { get; }
        public string NombreParaMostrar { get; }
        public string Token { get; }
        public string AvatarUrl { get; }

        public static LoginResult Failed => new LoginResult(false, null, null, null, null);
    }
}
