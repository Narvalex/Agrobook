using System.Linq;

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
        public string[] Claims { get; private set; }

        public void ActualizarPassword(string nuevoPassword)
        {
            this.Password = nuevoPassword;
        }

        public void RemoverClaim(string claim)
        {
            this.Claims = this.Claims.Where(x => x != claim).ToArray();
        }

        public void AddClaim(string claim)
        {
            var lista = this.Claims.ToList();
            lista.Add(claim);
            this.Claims = lista.ToArray();
        }
    }
}
