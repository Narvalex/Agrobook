namespace Agrobook.Domain.Usuarios.Login
{
    public class ClaimInfo
    {
        public ClaimInfo(string codigo, string nombre, string descripcion)
        {
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.Descripcion = descripcion;
        }

        public string Codigo { get; }
        public string Nombre { get; }
        public string Descripcion { get; }
    }
}
