using Agrobook.Domain.Ap.Commands;
using Eventing;
using Eventing.Core.Messaging;
using Eventing.Core.Persistence;
using Eventing.Log;
using System.Threading.Tasks;

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

        public async Task Handle(long checkpoint, NuevoRegistroDeServicioPendiente e)
        {
            if (this.repository.Exists<Servicio>(e.IdServicio).Result)
            {
                // This is the idemptency stuff
                this.logger.Warning("Ignorando mensaje ya manejado del tipo " + typeof(NuevoRegistroDeServicioPendiente).Name);
                return;
            }

            var cmd = new ProcesarRegistroDeServicioPendiente(e);
            this.apService.HandleAsync(cmd).Wait();
        }
    }
}
