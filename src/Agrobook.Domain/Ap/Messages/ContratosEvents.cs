using Agrobook.Core;

namespace Agrobook.Domain.Ap.Messages
{
    public class NuevoContrato : IEvent
    {
        public NuevoContrato(string idContrato, string idOrganizacion, string nombreDelContrato)
        {
            this.IdContrato = idContrato;
            this.IdOrganizacion = idOrganizacion;
            this.NombreDelContrato = nombreDelContrato;
        }

        /// <summary>
        /// Id contrato: [idOrg]_[nombre_contrato]
        /// </summary>
        public string IdContrato { get; }
        public string IdOrganizacion { get; }
        public string NombreDelContrato { get; }

        /// <summary>
        /// Todo lo que le ocurre al contrato
        /// </summary>
        public string StreamId => this.IdContrato;
    }

    public class NuevaAdenda : IEvent
    {
        public NuevaAdenda(string idOrganizacion, string idContrato, string idAdenda, string nombreDeLaAdenda)
        {
            this.IdContrato = idContrato;
            this.IdOrganizacion = idOrganizacion;
            this.IdAdenda = idAdenda;
            this.NombreDeLaAdenda = nombreDeLaAdenda;
        }

        public string IdContrato { get; }
        public string IdOrganizacion { get; }

        /// <summary>
        ///  Id adenda: [idContrato]_[nombre_adenda]
        /// </summary>
        public string IdAdenda { get; }
        public string NombreDeLaAdenda { get; }

        /// <summary>
        /// El contrato al que pertenece la adenda.
        /// </summary>
        public string StreamId => this.IdContrato;
    }
}
