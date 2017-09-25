using Agrobook.Common;
using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Ap.ServicioSaga;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public partial class ApService :
        IEventHandler<NuevoRegistroDeServicioPendiente>
    {
        public async Task HandleOnce(long eventNumber, NuevoRegistroDeServicioPendiente e)
        {
            var servicio = new Servicio();
            servicio.Emit(new NuevoServicioRegistrado(e.Firma, e.IdServicio, e.IdProd, e.IdOrg, e.IdContrato, e.Fecha));

            await this.repository.SaveAsync(servicio);
        }
    }
}
