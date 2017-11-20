using Agrobook.Domain.Common;
using Agrobook.Domain.Common.ValueObjects;

namespace Agrobook.Domain.Ap.Commands
{
    public class RegistrarParcela : MensajeAuditable
    {
        public RegistrarParcela(Firma firma, string idProductor, string nombreDeLaParcela, decimal hectareas, UbicacionDepartamental ubicacion) : base(firma)
        {
            this.IdProductor = idProductor;
            this.NombreDeLaParcela = nombreDeLaParcela;
            this.Hectareas = hectareas;
            this.Ubicacion = ubicacion;
        }

        public string IdProductor { get; }
        public string NombreDeLaParcela { get; }
        public decimal Hectareas { get; }
        public UbicacionDepartamental Ubicacion { get; }
    }

    public class EditarParcela : MensajeAuditable
    {
        public EditarParcela(Firma firma, string idProductor, string idParcela, string nombre, decimal hectareas, UbicacionDepartamental ubicacion)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
            this.Nombre = nombre;
            this.Hectareas = hectareas;
            this.Ubicacion = ubicacion;
        }

        public string IdProductor { get; }
        public string IdParcela { get; }
        public string Nombre { get; }
        public decimal Hectareas { get; }
        public UbicacionDepartamental Ubicacion { get; }
    }

    public class EliminarParcela : MensajeAuditable
    {
        public EliminarParcela(Firma firma, string idProductor, string idParcela)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
        }

        public string IdProductor { get; }
        public string IdParcela { get; }
    }

    public class RestaurarParcela : MensajeAuditable
    {
        public RestaurarParcela(Firma firma, string idProductor, string idParcela)
            : base(firma)
        {
            this.IdProductor = idProductor;
            this.IdParcela = idParcela;
        }

        public string IdProductor { get; }
        public string IdParcela { get; }
    }
}
