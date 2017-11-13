using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Ap.Commands
{
    public class ProcesarRegistroDeServicioPendiente
    {
        public ProcesarRegistroDeServicioPendiente(NuevoRegistroDeServicioPendiente registroPendiente)
        {
            this.RegistroPendiente = registroPendiente;
        }

        public NuevoRegistroDeServicioPendiente RegistroPendiente { get; }
    }

    public class EditarDatosBasicosDelSevicio : MensajeAuditable
    {
        public EditarDatosBasicosDelSevicio(Firma firma, string idServicio, string idOrg, string idContrato, bool esAdenda, string idContratoDeLaAdenda, DateTime fecha)
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

    public class EspecificarParcelaDelServicio : MensajeAuditable
    {
        public EspecificarParcelaDelServicio(Firma firma, string idServicio, string idParcela)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdParcela = idParcela;
        }

        public string IdServicio { get; }
        public string IdParcela { get; }
    }

    public class CambiarParcelaDelServicio : MensajeAuditable
    {
        public CambiarParcelaDelServicio(Firma firma, string idServicio, string idParcela)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.IdParcela = idParcela;
        }

        public string IdServicio { get; }
        public string IdParcela { get; }
    }

    public class FijarPrecioAlServicio : MensajeAuditable
    {
        public FijarPrecioAlServicio(Firma firma, string idServicio, decimal precio)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.Precio = precio;
        }

        public string IdServicio { get; }
        public decimal Precio { get; }
    }

    public class AjustarPrecioDelServicio : MensajeAuditable
    {
        public AjustarPrecioDelServicio(Firma firma, string idServicio, decimal precio)
            : base(firma)
        {
            this.IdServicio = idServicio;
            this.Precio = precio;
        }

        public string IdServicio { get; }
        public decimal Precio { get; }
    }
}
