namespace Agrobook.Domain.Common.ValueObjects
{
    public class UbicacionDepartamental
    {
        public UbicacionDepartamental(string idDepartamento, string idDistrito)
        {
            this.IdDepartamento = idDepartamento;
            this.IdDistrito = idDistrito;
        }

        public string IdDepartamento { get; }
        public string IdDistrito { get; }
    }
}
