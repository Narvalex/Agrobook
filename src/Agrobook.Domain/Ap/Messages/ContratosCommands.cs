namespace Agrobook.Domain.Ap.Messages
{
    public class RegistrarNuevoContrato
    {
        public RegistrarNuevoContrato(string idOrganizacion, string nombreDelContrato)
        {
            this.IdOrganizacion = idOrganizacion;
            this.NombreDelContrato = nombreDelContrato;
        }

        public string IdOrganizacion { get; }
        public string NombreDelContrato { get; }
    }

    public class RegistrarNuevaAdenda
    {
        public RegistrarNuevaAdenda(string idContrato, string nombreDeLaAdenda)
        {
            this.IdContrato = idContrato;
            this.NombreDeLaAdenda = nombreDeLaAdenda;
        }

        public string IdContrato { get; }
        public string NombreDeLaAdenda { get; }
    }
}
