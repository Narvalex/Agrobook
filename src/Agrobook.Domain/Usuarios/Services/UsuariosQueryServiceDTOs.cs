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
        /// <summary>
        /// Sirve para indicar si el usuario seleccionado es miembro
        /// </summary>
        public bool UsuarioEsMiembro { get; set; }
    }

    public class GrupoDto
    {
        public string Id { get; set; }
        public string Display { get; set; }
    }
}
