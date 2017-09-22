using Agrobook.Domain.Ap.Messages;
using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap.ServicioSaga
{
    [StreamCategory("agrobook.ap.servicioSecs")]
    public class ServicioSec : EventSourced
    {
        public ServicioSec()
        {
            this.On<NuevoServicioSec>(e => base.SetStreamNameById(e.IdProductor));
            this.On<NuevoRegistroDeServicioPendiente>(e => this.UltimoNroDeServicioDelProductor = e.NroDeServicioDelProd);
        }

        public int UltimoNroDeServicioDelProductor { get; private set; } = 0;

        protected override ISnapshot TakeSnapshot()
            => new ServicioSecSnapshot(this.StreamName, this.Version, this.UltimoNroDeServicioDelProductor);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ServicioSecSnapshot)snapshot;
            this.UltimoNroDeServicioDelProductor = state.UltimoNroDeServicioDelProductor;
        }
    }
}
