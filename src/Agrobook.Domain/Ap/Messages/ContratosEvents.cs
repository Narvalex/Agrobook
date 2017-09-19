using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class NuevoContrato : MensajeAuditable, IEvent
    {
        public NuevoContrato(Firma firma, string idContrato, string idOrganizacion, string nombreDelContrato, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdOrganizacion = idOrganizacion;
            this.NombreDelContrato = nombreDelContrato;
            this.Fecha = fecha;
        }

        /// <summary>
        /// Id contrato: [idOrg]_[nombre_contrato]
        /// </summary>
        public string IdContrato { get; }
        public string IdOrganizacion { get; }
        public string NombreDelContrato { get; }
        public DateTime Fecha { get; }

        /// <summary>
        /// Todo lo que le ocurre al contrato
        /// </summary>
        public string StreamId => this.IdContrato;
    }

    public class ContratoEditado : MensajeAuditable, IEvent
    {
        public ContratoEditado(Firma firma, string idContrato, string nombreDelContrato, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.NombreDelContrato = nombreDelContrato;
            this.Fecha = fecha;
        }

        public string IdContrato { get; }
        public string NombreDelContrato { get; }
        public DateTime Fecha { get; }

        public string StreamId => this.IdContrato;
    }

    public class ContratoEliminado : MensajeAuditable, IEvent
    {
        public ContratoEliminado(Firma firma, string idContrato) : base(firma)
        {
            this.IdContrato = idContrato;
        }

        public string IdContrato { get; }

        public string StreamId => this.IdContrato;
    }

    public class NuevaAdenda : MensajeAuditable, IEvent
    {
        public NuevaAdenda(Firma firma, string idOrganizacion, string idContrato, string idAdenda, string nombreDeLaAdenda, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdOrganizacion = idOrganizacion;
            this.IdAdenda = idAdenda;
            this.NombreDeLaAdenda = nombreDeLaAdenda;
            this.Fecha = fecha;
        }

        public string IdContrato { get; }
        public string IdOrganizacion { get; }

        /// <summary>
        ///  Id adenda: [idContrato]_[nombre_adenda]
        /// </summary>
        public string IdAdenda { get; }
        public string NombreDeLaAdenda { get; }
        public DateTime Fecha { get; }

        /// <summary>
        /// El contrato al que pertenece la adenda.
        /// </summary>
        public string StreamId => this.IdContrato;
    }

    public class AdendaEditada : MensajeAuditable, IEvent
    {
        public AdendaEditada(Firma firma, string idContrato, string idAdenda, string nombreDeLaAdenda, DateTime fecha)
            : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdAdenda = idAdenda;
            this.NombreDeLaAdenda = nombreDeLaAdenda;
            this.Fecha = fecha;
        }

        public string IdContrato { get; }
        public string IdAdenda { get; }
        public string NombreDeLaAdenda { get; }
        public DateTime Fecha { get; }

        public string StreamId => this.IdContrato;
    }

    public class AdendaEliminada : MensajeAuditable, IEvent
    {
        public AdendaEliminada(Firma firma, string idContrato, string idAdenda) : base(firma)
        {
            this.IdContrato = idContrato;
            this.IdAdenda = idAdenda;
        }

        public string IdContrato { get; }
        public string IdAdenda { get; }

        public string StreamId => this.IdContrato;
    }
}
