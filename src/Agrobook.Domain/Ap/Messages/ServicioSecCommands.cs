using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class RegistrarNuevoServicio : MensajeAuditable
    {
        public RegistrarNuevoServicio(Firma firma, string idProd, string idOrg, string idContrato, DateTime fecha,
            string orgDisplay, string contratoDisplay) : base(firma)
        {
            this.IdProd = idProd;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.Fecha = fecha;

            this.OrgDisplay = orgDisplay;
            this.ContratoDisplay = contratoDisplay;
        }

        public string IdProd { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }

        public string OrgDisplay { get; }
        public string ContratoDisplay { get; }
    }
}
