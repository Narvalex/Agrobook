using Agrobook.Domain.Ap.Messages;
using Eventing;
using Eventing.Core.Messaging;
using Eventing.Core.Persistence;
using Eventing.Log;

namespace Agrobook.Domain.Ap.Services
{
    public class NumeracionDeServiciosEventHandler : EventSourcedHandler,
        IHandler<NuevoRegistroDeServicioPendiente>
    {
        private readonly ILogLite logger = LogManager.GetLoggerFor<NumeracionDeServiciosEventHandler>();
        private readonly ApService apService;

        public NumeracionDeServiciosEventHandler(IEventSourcedRepository repository, ApService apService) : base(repository)
        {
            Ensure.NotNull(apService, nameof(apService));

            this.apService = apService;
        }

        public void Handle(long checkpoint, NuevoRegistroDeServicioPendiente e)
        {
            if (this.repository.Exists<Servicio>(e.IdServicio).Result)
            {
                // This is the idemptency stuff
                this.logger.Warning("Ignorando mensaje ya manejado del tipo " + typeof(NuevoRegistroDeServicioPendiente).Name);
                return;
            }

            this.apService.AsegurarQueElContratoOLaAdendaSeanValidos(e.EsAdenda, e.IdContrato, e.IdContratoDeLaAdenda).Wait();

            var servicio = new Servicio();

            servicio.Emit(new NuevoServicioRegistrado(
                e.Firma, e.IdServicio, e.IdProd, e.IdOrg, e.IdContrato, e.EsAdenda, e.IdContratoDeLaAdenda, e.Fecha));

            this.repository.SaveAsync(servicio).Wait();
        }
    }
}
