using Agrobook.Domain.Ap.Messages;
using Eventing.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    // Contratos
    public partial class ApService
    {
        public async Task HandleAsync(RegistrarNuevoContrato cmd)
        {
            var contrato = new Contrato();

            if (string.IsNullOrWhiteSpace(cmd.IdOrganizacion))
                throw new InvalidOperationException("La organización debe estar especificada.");

            if (string.IsNullOrWhiteSpace(cmd.NombreDelContrato))
                throw new InvalidOperationException("El nombre debe estar especificado");

            var idContrato = $"{cmd.IdOrganizacion.ToTrimmedAndWhiteSpaceless()}_{cmd.NombreDelContrato.ToTrimmedAndWhiteSpaceless()}";
            contrato.Emit(new NuevoContrato(idContrato, cmd.IdOrganizacion, cmd.NombreDelContrato));

            await this.repository.SaveAsync(contrato);
        }

        public async Task HandleAsync(RegistrarNuevaAdenda cmd)
        {
            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);

            //contrato.Emit(new NuevaAdenda(cmd.))
        }
    }
}
