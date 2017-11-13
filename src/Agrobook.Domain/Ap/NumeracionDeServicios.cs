using Agrobook.Domain.Ap.Commands;
using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap
{
    /// <summary>
    /// Proyeccion utilizada para numerar los servicios correspondientes a un productor. 
    /// Por cada nuevo pedido se utiliza el siguiente numero, empezando del uno.
    /// </summary>
    /// <remarks>
    /// Esta proyeccion no se encarga de generar los identificadores, solamente los numeros consecutivos.
    /// </remarks>
    [StreamCategory("agrobook.ap.numeracionesDeServicios")]
    public class NumeracionDeServicios : EventSourced
    {
        public NumeracionDeServicios()
        {
            this.On<NumeracionDeServiciosIniciada>(e => base.SetStreamNameById(e.IdProductor));
            this.On<NuevoRegistroDeServicioPendiente>(e => this.UltimoNroDeServicioDelProductor++);
        }

        public int UltimoNroDeServicioDelProductor { get; private set; } = 0;

        protected override ISnapshot TakeSnapshot()
            => new NumeracionDeServiciosSnapshot(this.StreamName, this.Version, this.UltimoNroDeServicioDelProductor);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (NumeracionDeServiciosSnapshot)snapshot;
            this.UltimoNroDeServicioDelProductor = state.UltimoNroDeServicioDelProductor;
        }
    }
}
