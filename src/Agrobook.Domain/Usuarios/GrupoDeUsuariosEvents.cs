namespace Agrobook.Domain.Usuarios
{
    public class NuevoGrupoCreado
    {
        public NuevoGrupoCreado(string idGrupo)
        {
            this.IdGrupo = idGrupo;
        }

        public string IdGrupo { get; }
    }

    public class UsuarioAgregadoAGrupo
    {
        public UsuarioAgregadoAGrupo(string idGrupo, string idUsuario)
        {
            this.IdGrupo = idGrupo;
            this.IdUsuario = idUsuario;
        }

        public string IdGrupo { get; }
        public string IdUsuario { get; }
    }
}
