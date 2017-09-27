using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Messages
{
    public class EditarDatosBasicosDelSevicio : MensajeAuditable
    {
        public EditarDatosBasicosDelSevicio(Firma firma, string idServicio, string idOrg, string idContrato, DateTime fecha)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.Fecha = fecha;
        }

        public string IdServicio { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }
    }

    public class EliminarServicio : MensajeAuditable
    {
        public EliminarServicio(Firma firma, string idServicio)
            : base(firma)
        {
            this.IdServicio = idServicio;
        }

        public string IdServicio { get; }
    }

    public class RestaurarServicio : MensajeAuditable
    {
        public RestaurarServicio(Firma firma, string idServicio)
            : base(firma)
        {
            this.IdServicio = idServicio;
        }

        public string IdServicio { get; }
    }
}
