using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class NuevoServicioRegistrado : MensajeAuditable, IEvent
    {
        public NuevoServicioRegistrado(Firma firma, string idServicio, string idProd, string idOrg, string idContrato, DateTime fecha)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdProd = idProd;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.Fecha = fecha;
        }

        /// <summary>
        /// El id del servicio se lee asi: [idProd]_[numeroQueLeSigue]
        /// </summary>
        public string IdServicio { get; }
        public string IdProd { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }

        public string StreamId => this.IdServicio;
    }
}
