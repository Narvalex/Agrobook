namespace Agrobook.Domain.Usuarios
{
    public class CrearGrupo
    {
        public CrearGrupo(string idGrupo)
        {
            this.IdGrupo = idGrupo;
        }

        public string IdGrupo { get; }
    }

    public class AgregarUsuarioAGrupo
    {
        public AgregarUsuarioAGrupo(string idGrupo, string idUsuario)
        {
            this.IdGrupo = idGrupo;
            this.IdUsuario = idUsuario;
        }

        public string IdGrupo { get; }
        public string IdUsuario { get; }
    }
}
