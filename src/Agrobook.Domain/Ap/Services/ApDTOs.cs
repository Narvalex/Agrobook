namespace Agrobook.Domain.Ap.Services
{
    public class ClienteDeAp
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Desc { get; set; }
        public string Tipo { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class OrgDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string AvatarUrl { get; set; }
    }
}
