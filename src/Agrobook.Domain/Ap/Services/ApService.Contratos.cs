using Agrobook.Domain.Ap.Messages;
using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;
namespace Agrobook.Domain.Ap.Services
{
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

        public async Task HandleAsync(EliminarContrato cmd)
        {
            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.Id);

            if (contrato.EstaEliminado)
                throw new InvalidOperationException("El contrato ya está eliminado");

            contrato.Emit(new ContratoEliminado(cmd.Firma, cmd.Id));

            await this.repository.SaveAsync(contrato);
        }

        public async Task HandleAsync(RestaurarContrato cmd)
        {
            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.Id);

            if (!contrato.EstaEliminado)
                throw new InvalidOperationException("No se puede restaurar contrato que no está eliminado");

            contrato.Emit(new ContratoRestaurado(cmd.Firma, cmd.Id));

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

        public async Task HandleAsync(EliminarAdenda cmd)
        {
            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);

            if (!contrato.TieneAdenda(cmd.IdAdenda))
                throw new InvalidOperationException("El contrato no tiene esa adenda");
            if (contrato.LaAdendaEstaEliminada(cmd.IdAdenda))
                throw new InvalidOperationException("La adenda ya esta eliminada");

            contrato.Emit(new AdendaEliminada(cmd.Firma, cmd.IdContrato, cmd.IdAdenda));

            await this.repository.SaveAsync(contrato);
        }

        public async Task HandleAsync(RestaurarAdenda cmd)
        {
            var contrato = await this.repository.GetOrFailByIdAsync<Contrato>(cmd.IdContrato);

            if (!contrato.TieneAdenda(cmd.IdAdenda))
                throw new InvalidOperationException("El contrato ni siquiere tiene la adenda que se quiere restaurar");

            if (!contrato.LaAdendaEstaEliminada(cmd.IdAdenda))
                throw new InvalidOperationException("La adenda no esta eliminada como para restaurar");

            contrato.Emit(new AdendaRestaurada(cmd.Firma, cmd.IdContrato, cmd.IdAdenda));

            await this.repository.SaveAsync(contrato);
        }
    }
}
