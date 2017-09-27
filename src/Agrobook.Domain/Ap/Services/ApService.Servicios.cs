using Agrobook.Common;
using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Ap.ServicioSaga;
using Agrobook.Domain.Usuarios;
using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public partial class ApService :
        IEventHandler<NuevoRegistroDeServicioPendiente>
    {
        // explicity implemented to guard a missusing of the api by a client. 
        // This should only has to be called by the event subscription stuff
        async Task IEventHandler<NuevoRegistroDeServicioPendiente>.HandleOnce(long eventNumber, NuevoRegistroDeServicioPendiente e)
        {
            var servicio = new Servicio();
            servicio.Emit(new NuevoServicioRegistrado(e.Firma, e.IdServicio, e.IdProd, e.IdOrg, e.IdContrato, e.Fecha));

            await this.repository.SaveAsync(servicio);
        }

        public async Task HandleAsync(EditarDatosBasicosDelSevicio cmd)
        {
            await this.repository.EnsureExistenceOf<Organizacion>(cmd.IdOrg)
                .And<Contrato>(cmd.IdContrato)
                .AndNothingMore();

            cmd.Fecha.EnsureIsNotDefault(nameof(cmd.Fecha));

            var servicio = await this.repository.GetOrFailByIdAsync<Servicio>(cmd.IdServicio);

            servicio.Emit(new DatosBasicosDelSevicioEditados(cmd.Firma, cmd.IdServicio, cmd.IdOrg, cmd.IdContrato, cmd.Fecha));

            await this.repository.SaveAsync(servicio);
        }

        public async Task HandleAsync(EliminarServicio cmd)
        {
            var servicio = await this.repository.GetOrFailByIdAsync<Servicio>(cmd.IdServicio);

            if (servicio.EstaEliminado)
                throw new InvalidOperationException("El servicio ya esta luego eliminado!");

            servicio.Emit(new ServicioEliminado(cmd.Firma, cmd.IdServicio));

            await this.repository.SaveAsync(servicio);
        }

        public async Task HandleAsync(RestaurarServicio cmd)
        {
            var servicio = await this.repository.GetOrFailByIdAsync<Servicio>(cmd.IdServicio);

            if (!servicio.EstaEliminado)
                throw new InvalidOperationException("El servicio no esta eliminado. No necesita restaurarse");

            servicio.Emit(new ServicioRestaurado(cmd.Firma, cmd.IdServicio));

            await this.repository.SaveAsync(servicio);
        }
    }
}
