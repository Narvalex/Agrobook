using Agrobook.Domain.Ap.Messages;
using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap.ServicioSaga
{
    [StreamCategory("agrobook.ap.servicios")]
    public class Servicio : EventSourced
    {
        public Servicio()
        {
            this.On<NuevoServicioRegistrado>(e => this.SetStreamNameById(e.StreamId));
        }
    }
}
