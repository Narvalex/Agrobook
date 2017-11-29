using Agrobook.Domain.Ap.Commands;
using Agrobook.Domain.Ap.ValueObjects;
using Agrobook.Domain.Common.Services;
using Eventing;
using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    /// <summary>
    /// Servicio del Productor. Quizás debiera tener su propio servicio por separado, en vez de ser una clase 
    /// parcial de <see cref="Agrobook.Domain.Ap.Services.ApService"/>. Pero por ahora se puede trabajar bien 
    /// así. He visto clases muuuucho más grandes.
    /// </summary>
    /// <remarks>
    /// Un productor se registra en el sistema, cuando a un usuario se le registra una parcela.
    /// </remarks>
    public partial class ApService
    {
        public async Task<string> HandleAsync(RegistrarParcela cmd)
        {
            // we dont trust user input...
            var idProductor = cmd.IdProductor;
            var productor = await this.repository.GetByIdAsync<Productor>(idProductor);
            if (productor is null)
            {
                productor = new Productor();
                await ApIdProvider.ValidarQueExistaElUsuario(idProductor, this.repository);
                productor.Emit(new NuevoProductorRegistrado(cmd.Firma, idProductor));
            }

            // prod + nombreDeParcela
            var idParcela = $"{idProductor}_{cmd.NombreDeLaParcela.ToTrimmedAndWhiteSpaceless()}";

            if (productor.TieneParcela(idParcela))
                throw new InvalidOperationException("El productor ya tiene esa parcela");

            Ensure.Positive(cmd.Hectareas, nameof(cmd.Hectareas));

            if (!DepartamentosDelParaguayProvider.UbicacionEsValida(cmd.Ubicacion))
                throw new ArgumentException("No es valida la ubicacion departamental", "ubicacion departamental");

            productor.Emit(new NuevaParcelaRegistrada(cmd.Firma, idProductor, idParcela, cmd.NombreDeLaParcela, cmd.Hectareas, cmd.Ubicacion));

            await this.repository.SaveAsync(productor);
            return idParcela;
        }

        public async Task HandleAsync(EditarParcela cmd)
        {
            var productor = await this.repository.GetOrFailByIdAsync<Productor>(cmd.IdProductor);

            if (!productor.TieneParcela(cmd.IdParcela))
                throw new InvalidOperationException("El productor no tiene la parcela que se quiere editar");

            // Verificamos si siquiera hay diferencia
            var parcelaActual = productor.MirarParcela(cmd.IdParcela);
            if (!parcelaActual.EsDiferenteDe(new Parcela(cmd.IdParcela, cmd.Hectareas, cmd.Ubicacion, parcelaActual.Eliminada)))
                throw new InvalidOperationException("No hay diferencias con la parcela actual");

            // Ya que se encontro diferencias, entonces se puede publicar que se edito algo
            Ensure.Positive(cmd.Hectareas, nameof(cmd.Hectareas));

            if (!DepartamentosDelParaguayProvider.UbicacionEsValida(cmd.Ubicacion))
                throw new ArgumentException("No es valida la ubicacion departamental", "ubicacion departamental");

            productor.Emit(new ParcelaEditada(cmd.Firma, cmd.IdProductor, cmd.IdParcela, cmd.Nombre, cmd.Hectareas, cmd.Ubicacion));

            await this.repository.SaveAsync(productor);
        }

        public async Task HandleAsync(EliminarParcela cmd)
        {
            var productor = await this.repository.GetOrFailByIdAsync<Productor>(cmd.IdProductor);

            if (!productor.TieneParcela(cmd.IdParcela))
                throw new InvalidOperationException("El productor no tiene la parcela que se quiere eliminar");

            if (productor.ParcelaEstaEliminada(cmd.IdParcela))
                throw new InvalidOperationException("La parcela esta eliminada luego... No se puede eliminar asi");

            productor.Emit(new ParcelaEliminada(cmd.Firma, cmd.IdProductor, cmd.IdParcela));

            await this.repository.SaveAsync(productor);
        }

        public async Task HandleAsync(RestaurarParcela cmd)
        {
            var productor = await this.repository.GetOrFailByIdAsync<Productor>(cmd.IdProductor);

            if (!productor.TieneParcela(cmd.IdParcela))
                throw new InvalidOperationException("El productor no tiene la parcela que se quiere restaurar");

            if (!productor.ParcelaEstaEliminada(cmd.IdParcela))
                throw new InvalidOperationException("La parcela no esta eliminada. No se puede restaurar algo que no esta eliminado");

            productor.Emit(new ParcelaRestaurada(cmd.Firma, cmd.IdProductor, cmd.IdParcela));

            await this.repository.SaveAsync(productor);
        }
    }
}
