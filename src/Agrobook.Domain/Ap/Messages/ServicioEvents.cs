using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class NuevoServicioRegistrado : MensajeAuditable, IEvent
    {
        public NuevoServicioRegistrado(Firma firma, string idServicio, string idProd, string idOrg, string idContrato, DateTime fecha,
            string orgDisplay, string contratoDisplay)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdProd = idProd;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.Fecha = fecha;
            this.OrgDisplay = orgDisplay;
            this.ContratoDisplay = contratoDisplay;
        }

        /// <summary>
        /// El id del servicio se lee asi: [idProd]_[numeroQueLeSigue]
        /// </summary>
        public string IdServicio { get; }
        public string IdProd { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }

        // Extra info
        public string OrgDisplay { get; }
        public string ContratoDisplay { get; }

        public string StreamId => this.IdServicio;
    }
}
