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

    public class DatosBasicosDelSevicioEditados : MensajeAuditable, IEvent
    {
        public DatosBasicosDelSevicioEditados(Firma firma, string idServicio, string idOrg, string idContrato, DateTime fecha)
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

        public string StreamId => this.IdServicio;
    }

    public class ServicioEliminado : MensajeAuditable, IEvent
    {
        public ServicioEliminado(Firma firma, string idServicio)
            : base(firma)
        {
            this.IdServicio = idServicio;
        }

        public string IdServicio { get; }

        public string StreamId => this.IdServicio;
    }

    public class ServicioRestaurado : MensajeAuditable, IEvent
    {
        public ServicioRestaurado(Firma firma, string idServicio)
            : base(firma)
        {
            this.IdServicio = idServicio;
        }

        public string IdServicio { get; }

        public string StreamId => this.IdServicio;
    }

    public class ParcelaDeServicioEspecificada : MensajeAuditable, IEvent
    {
        public ParcelaDeServicioEspecificada(Firma firma, string idServicio, string idParcela)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdParcela = idParcela;
        }

        public string IdServicio { get; }
        public string IdParcela { get; }

        public string StreamId => this.IdServicio;
    }

    public class ParcelaDeServicioCambiada : MensajeAuditable, IEvent
    {
        public ParcelaDeServicioCambiada(Firma firma, string idServicio, string idParcela)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdParcela = idParcela;
        }

        public string IdServicio { get; }
        public string IdParcela { get; }

        public string StreamId => this.IdServicio;
    }
}
