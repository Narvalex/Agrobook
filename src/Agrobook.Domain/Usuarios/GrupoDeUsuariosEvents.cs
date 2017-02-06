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
}
