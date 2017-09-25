using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class RegistrarNuevoServicio : MensajeAuditable
    {
        public RegistrarNuevoServicio(Firma firma, string idProd, string idOrg, string idContrato, DateTime fecha)
            : base(firma)
        {
            this.IdProd = idProd;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.Fecha = fecha;
        }

        public string IdProd { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }
    }
}
