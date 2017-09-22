using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class NuevoServicioSec : MensajeAuditable, IEvent
    {
        public NuevoServicioSec(Firma firma, string idProductor) : base(firma)
        {
            this.IdProductor = idProductor;
        }

        public string IdProductor { get; }
        public string StreamId => this.IdProductor;
    }

    public class NuevoRegistroDeServicioPendiente : MensajeAuditable, IEvent
    {
        public NuevoRegistroDeServicioPendiente(Firma firma, string idProd, int nroDeServicioDelProd, string idServicio, string idOrg, string idContrato, DateTime fecha,
            string orgDisplay, string contratoDisplay) : base(firma)
        {
            this.IdProd = idProd;
            this.NroDeServicioDelProd = nroDeServicioDelProd;
            this.IdServicio = idServicio;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.Fecha = fecha;

            this.OrgDisplay = orgDisplay;
            this.ContratoDisplay = contratoDisplay;
        }

        public string IdProd { get; }
        public int NroDeServicioDelProd { get; }
        public string IdServicio { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }

        public string OrgDisplay { get; set; }
        public string ContratoDisplay { get; set; }

        public string StreamId => this.IdProd;
    }
}
