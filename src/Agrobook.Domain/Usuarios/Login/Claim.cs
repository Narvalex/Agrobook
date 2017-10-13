namespace Agrobook.Domain.Usuarios.Login
{
    public class Claim
    {
        public Claim(string id, TipoDeClaim tipo, string display, string info)
        {
            this.Id = id;
            this.Tipo = tipo;
            this.Display = display;
            this.Info = info;
            this.Tipo = tipo;
        }

        public string Id { get; }
        public TipoDeClaim Tipo { get; }
        public string Display { get; }
        public string Info { get; }
    }

    public enum TipoDeClaim
    {
        Rol, Permiso
    }
}
