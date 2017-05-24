namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuarioInfoBasica
    {
        public string Nombre { get; set; }
        public string NombreParaMostrar { get; set; }
        public string AvatarUrl { get; set; }
    }

    public class OrganizacionDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
    }

    public class ClaimDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
        public string Info { get; set; }
    }
}
