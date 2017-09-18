using Agrobook.Domain.Ap.Messages;
using Eventing;
using Eventing.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    // Contratos
    public partial class ApService
    {
        public async Task<string> HandleAsync(RegistrarNuevoContrato cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd.IdOrganizacion))
                throw new InvalidOperationException("La organización debe estar especificada.");

            if (string.IsNullOrWhiteSpace(cmd.NombreDelContrato))
                throw new InvalidOperationException("El nombre debe estar especificado");

            cmd.Fecha.EnsureIsNotDefault("fecha del contrato");

            var contrato = new Contrato();

            var idContrato = $"{cmd.IdOrganizacion.ToTrimmedAndWhiteSpaceless()}_{cmd.NombreDelContrato.ToTrimmedAndWhiteSpaceless()}";
            contrato.Emit(new NuevoContrato(cmd.Firma, idContrato, cmd.IdOrganizacion, cmd.NombreDelContrato, cmd.Fecha));

            await this.repository.SaveAsync(contrato);
            return idContrato;
        }

        public async Task<string> HandleAsync(RegistrarNuevaAdenda cmd)
        {
            Ensure.NotNullOrWhiteSpace(cmd.IdContrato, nameof(cmd.IdContrato));
            Ensure.NotNullOrWhiteSpace(cmd.NombreDeLaAdenda, nameof(cmd.NombreDeLaAdenda));

            cmd.Fecha.EnsureIsNotDefault("Fecha de la adenda");

            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);

            var idAdenda = $"{cmd.IdContrato}_{cmd.NombreDeLaAdenda.ToTrimmedAndWhiteSpaceless()}";
            contrato.Emit(new NuevaAdenda(cmd.Firma, contrato.IdOrganizacion, cmd.IdContrato, idAdenda, cmd.NombreDeLaAdenda, cmd.Fecha));

            await this.repository.SaveAsync(contrato);
            return idAdenda;
        }
    }
}
