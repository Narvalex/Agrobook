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

            //cmd.Fecha.EnsureIsNotDefault("fecha del contrato");
            var fecha = cmd.Fecha == default(DateTime) ? this.dateTimeProvider.Now : cmd.Fecha;

            var contrato = new Contrato();

            var idContrato = $"{cmd.IdOrganizacion.ToTrimmedAndWhiteSpaceless()}_{cmd.NombreDelContrato.ToTrimmedAndWhiteSpaceless()}";
            contrato.Emit(new NuevoContrato(cmd.Firma, idContrato, cmd.IdOrganizacion, cmd.NombreDelContrato, fecha));

            await this.repository.SaveAsync(contrato);
            return idContrato;
        }

        public async Task HandleAsync(EditarContrato cmd)
        {
            cmd.Fecha.EnsureIsNotDefault("Fecha del contrato");
            Ensure.NotNullOrWhiteSpace(cmd.NombreDelContrato, "nombre del contrato");

            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);
            contrato.Emit(new ContratoEditado(cmd.Firma, cmd.IdContrato, cmd.NombreDelContrato, cmd.Fecha));

            await this.repository.SaveAsync(contrato);
        }

        public async Task<string> HandleAsync(RegistrarNuevaAdenda cmd)
        {
            Ensure.NotNullOrWhiteSpace(cmd.IdContrato, nameof(cmd.IdContrato));
            Ensure.NotNullOrWhiteSpace(cmd.NombreDeLaAdenda, nameof(cmd.NombreDeLaAdenda));

            var fecha = cmd.Fecha == default(DateTime) ? this.dateTimeProvider.Now : cmd.Fecha;

            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);

            var idAdenda = $"{cmd.IdContrato}_{cmd.NombreDeLaAdenda.ToTrimmedAndWhiteSpaceless()}";

            if (contrato.TieneAdenda(idAdenda))
                throw new InvalidOperationException("La adenda ya existe");

            contrato.Emit(new NuevaAdenda(cmd.Firma, contrato.IdOrganizacion, cmd.IdContrato, idAdenda, cmd.NombreDeLaAdenda, fecha));

            await this.repository.SaveAsync(contrato);
            return idAdenda;
        }

        public async Task HandleAsync(EditarAdenda cmd)
        {
            cmd.Fecha.EnsureIsNotDefault("Fecha adenda");
            Ensure.NotNullOrWhiteSpace(cmd.NombreDeLaAdenda, nameof(cmd.NombreDeLaAdenda));

            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);
            if (!contrato.TieneAdenda(cmd.IdAdenda))
                throw new InvalidOperationException("La adenda que se quiere editar no existe");

            contrato.Emit(new AdendaEditada(cmd.Firma, cmd.IdContrato, cmd.IdAdenda, cmd.NombreDeLaAdenda, cmd.Fecha));

            await this.repository.SaveAsync(contrato);
        }
    }
}
