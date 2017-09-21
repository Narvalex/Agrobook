using Agrobook.Core;
using Agrobook.Domain.Common;

namespace Agrobook.Domain.Ap.Messages
{
    public class NuevoProductorRegistrado : MensajeAuditable, IEvent
    {
        public NuevoProductorRegistrado(Firma firma, string idProductor)
            : base(firma)
        {
            this.IdProductor = idProductor;
        }

        public string IdProductor { get; }
        public string StreamId => this.IdProductor;
    }

    public class NuevaParcelaRegistrada : MensajeAuditable, IEvent
    {
        public NuevaParcelaRegistrada(Firma firma, string idProductor, string idParcela, string nombreDeLaParcela, decimal hectareas)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
            this.NombreDeLaParcela = nombreDeLaParcela;
            this.Hectareas = hectareas;
        }
        public string IdProductor { get; }
        public string IdParcela { get; }
        public string NombreDeLaParcela { get; }
        public decimal Hectareas { get; }

        public string StreamId => this.IdProductor;
    }

    public class ParcelaEditada : MensajeAuditable, IEvent
    {
        public ParcelaEditada(Firma firma, string idProductor, string idParcela, string nombre, decimal hectareas)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
            this.Nombre = nombre;
            this.Hectareas = hectareas;
        }

        public string IdProductor { get; }
        public string IdParcela { get; }
        public string Nombre { get; }
        public decimal Hectareas { get; }

        public string StreamId => this.IdProductor;
    }

    public class ParcelaEliminada : MensajeAuditable, IEvent
    {
        public ParcelaEliminada(Firma firma, string idProductor, string idParcela)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
        }

        public string IdProductor { get; }
        public string IdParcela { get; }

        public string StreamId => this.IdProductor;
    }

    public class ParcelaRestaurada : MensajeAuditable, IEvent
    {
        public ParcelaRestaurada(Firma firma, string idProductor, string idParcela)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
        }

        public string IdProductor { get; }
        public string IdParcela { get; }

        public string StreamId => this.IdProductor;
    }
}
