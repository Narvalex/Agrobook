using Agrobook.Domain.Ap.Messages;
using Agrobook.Domain.Ap.ServicioSaga;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public partial class ApService
    {
        public async Task<string> HandleAsync(RegistrarNuevoServicio cmd)
        {
            var servicioSec = await this.repository.GetAsync<ServicioSec>(cmd.IdProd);
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
                cmd.Fecha));

            await this.repository.SaveAsync(servicioSec);

            return idServicio;
        }
    }
}
