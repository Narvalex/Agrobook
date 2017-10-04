using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Ap.ServicioSaga;
using Agrobook.Domain.Usuarios;
using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public partial class ApService
    {
        public async Task<string> HandleAsync(RegistrarNuevoServicio cmd)
        {
            await this.repository
                .EnsureExistenceOf<Organizacion>(cmd.IdOrg)
                // .And<Productor>(cmd.IdProd) --- Esto comentamos porque recien es un productor cuando tiene parcela
                .And<Usuario>(cmd.IdProd)
                .And<Contrato>(cmd.EsAdenda ? cmd.IdContratoDeLaAdenda : cmd.IdContrato)
                .AndNothingMore();

            var servicioSec = await this.repository.GetByIdAsync<ServicioSec>(cmd.IdProd);
            if (servicioSec is null)
            {
                servicioSec = new ServicioSec();
                servicioSec.Emit(new NuevoServicioSec(cmd.Firma, cmd.IdProd));
            }

            var nroDeServicio = servicioSec.UltimoNroDeServicioDelProductor + 1;
            var idServicio = $"{cmd.IdProd.ToTrimmedAndWhiteSpaceless()}_{nroDeServicio}";
            servicioSec.Emit(new NuevoRegistroDeServicioPendiente(
                cmd.Firma,
                cmd.IdProd,
                nroDeServicio,
                idServicio,
                cmd.IdOrg,
                cmd.IdContrato,
                cmd.EsAdenda,
                cmd.IdContratoDeLaAdenda,
                cmd.Fecha));

            await this.repository.SaveAsync(servicioSec);

            return idServicio;
        }
    }
}
