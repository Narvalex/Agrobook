namespace Agrobook.Client.Usuarios
{
    public class UsuarioDto
    {
        public string AvatarUrl { get; set; }
        public string NombreDeUsuario { get; set; }
        public string NombreParaMostrar { get; set; }
        public string Password { get; set; }
        public string[] Claims { get; set; }
    }

    public class ActualizarPerfilDto
    {
        public string Usuario { get; set; }
        public string AvatarUrl { get; set; }
        public string NombreParaMostrar { get; set; }
        public string PasswordActual { get; set; }
        public string NuevoPassword { get; set; }
    }
}
