using Agrobook.Core;
using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap
{
    public class NuevoServicioRegistrado : MensajeAuditable, IEvent
    {
        public NuevoServicioRegistrado(Firma firma, string idServicio, string idProd, string idOrg,
            string idContrato, bool esAdenda, string idContratoDeLaAdenda, DateTime fecha)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdProd = idProd;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.EsAdenda = esAdenda;
            this.IdContratoDeLaAdenda = idContratoDeLaAdenda;
            this.Fecha = fecha;
        }

        /// <summary>
        /// El id del servicio se lee asi: [idProd]_[numeroQueLeSigue]
        /// </summary>
        public string IdServicio { get; }
        public string IdProd { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public bool EsAdenda { get; }
        public string IdContratoDeLaAdenda { get; }
        public DateTime Fecha { get; }

        public string StreamId => this.IdServicio;
    }

    public class DatosBasicosDelSevicioEditados : MensajeAuditable, IEvent
    {
        public DatosBasicosDelSevicioEditados(Firma firma, string idServicio, string idOrg,
            string idContrato, bool esAdenda, string idContratoDeLaAdenda, DateTime fecha)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdOrg = idOrg;
            this.IdContrato = idContrato;
            this.EsAdenda = esAdenda;
            this.IdContratoDeLaAdenda = idContratoDeLaAdenda;
            this.Fecha = fecha;
        }

        public string IdServicio { get; }
        public string IdOrg { get; }
        public string IdContrato { get; }
        public bool EsAdenda { get; }
        public string IdContratoDeLaAdenda { get; }
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

    /// <summary>
    /// Confirma que se especificó una parcela, tal cual estaba en el momento en que se especificó. 
    /// Si la parcela cambia de estado, o se modifica el número de hectáreas, esto no afecta el registro
    /// del servicio, por que se supone que la parcela se cambió DESPUÉS del servicio.
    /// </summary>
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

    public class PrecioDeServicioFijado : MensajeAuditable, IEvent
    {
        public PrecioDeServicioFijado(Firma firma, string idServicio, string moneda, decimal precioTotal)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.PrecioTotal = precioTotal;
            this.Moneda = moneda;
        }

        public string IdServicio { get; }
        public string Moneda { get; }
        public decimal PrecioTotal { get; }

        public string StreamId => this.IdServicio;
    }

    public class PrecioDeServicioAjustado : MensajeAuditable, IEvent
    {
        public PrecioDeServicioAjustado(Firma firma, string idServicio, string moneda, decimal precioTotal)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.PrecioTotal = precioTotal;
            this.Moneda = moneda;
        }

        public string IdServicio { get; }
        public string Moneda { get; }
        public decimal PrecioTotal { get; }

        public string StreamId => this.IdServicio;
    }
}
