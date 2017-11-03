using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Usuarios;
using Eventing.Core.Persistence;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public class NumeracionDeServiciosCommandHandler : EventSourcedHandler
    {
        public NumeracionDeServiciosCommandHandler(IEventSourcedRepository repository) : base(repository)
        {
        }

        public async Task<string> HandleAsync(RegistrarNuevoServicio cmd)
        {
            await this.repository
                .EnsureExistenceOf<Organizacion>(cmd.IdOrg)
                .And<Contrato>(cmd.EsAdenda ? cmd.IdContratoDeLaAdenda : cmd.IdContrato)
                .AndNothingMore();

            var numeracion = await this.repository.GetByIdAsync<NumeracionDeServicios>(cmd.IdProd);
            if (numeracion is null)
            {
                numeracion = new NumeracionDeServicios();
                await ApIdProvider.ValidarIdDeNumeracionDeServiciosPorProductor(cmd.IdProd, this.repository);
                numeracion.Emit(new NumeracionDeServiciosIniciada(cmd.Firma, cmd.IdProd));
            }

            var idServicio = await ApIdProvider.ObtenerNuevoIdDeServicio(numeracion.UltimoNroDeServicioDelProductor, cmd.IdProd, this.repository);
            numeracion.Emit(new NuevoRegistroDeServicioPendiente(
                cmd.Firma,
                cmd.IdProd,
                idServicio,
                cmd.IdOrg,
                cmd.IdContrato,
                cmd.EsAdenda,
                cmd.IdContratoDeLaAdenda,
                cmd.Fecha));

            await this.repository.SaveAsync(numeracion);

            return idServicio;
        }
    }
}
