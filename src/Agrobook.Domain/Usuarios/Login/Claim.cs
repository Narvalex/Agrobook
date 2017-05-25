namespace Agrobook.Domain.Usuarios.Login
{
    public class Claim
    {
        public Claim(string id, string display, string info)
        {
            this.Id = id;
            this.Display = display;
            this.Info = info;
        }

        public string Id { get; }
        public string Display { get; }
        public string Info { get; }
    }
}
