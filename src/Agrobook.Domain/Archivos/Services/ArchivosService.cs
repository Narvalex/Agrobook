using Eventing;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosService : EventSourcedService
    {
        private readonly IFileWriter fileWriter;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public ArchivosService(IFileWriter fileWriter, IEventSourcedRepository repository) : base(repository)
        {
            Ensure.NotNull(fileWriter, nameof(fileWriter));

            this.fileWriter = fileWriter;
        }

        public async Task<ResultadoDelUpload> HandleAsync(AgregarArchivoAColeccion cmd)
        {
            var @lock = this.locks.GetOrAdd(cmd.idColeccion, new SemaphoreSlim(1, 1));
            await @lock.WaitAsync();
            try
            {
                var resultado = await HandleAsyncWithPesimisticConcurrencyLock(cmd);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                // This is the optimization of the locking mechanism. 
                // Requires more testing
                if (@lock.CurrentCount < 1)
                    this.locks.TryRemove(cmd.idColeccion, out @lock);
                @lock?.Release();
            }
        }

        public async Task HandleAsync(RegistrarDescargaExitosa cmd)
        {
            var coleccion = await this.repository.GetOrFailByIdAsync<ColeccionDeArchivos>(cmd.IdColeccion);
            coleccion.Emit(new ArchivoDescargadoExitosamente(cmd.Firma, cmd.IdColeccion, cmd.NombreArchivo, coleccion.GetSize(cmd.NombreArchivo)));

            await this.repository.SaveAsync(coleccion);
        }

        private async Task<ResultadoDelUpload> HandleAsyncWithPesimisticConcurrencyLock(AgregarArchivoAColeccion cmd)
        {
            var coleccion = await this.repository.GetByIdAsync<ColeccionDeArchivos>(cmd.idColeccion);
            if (coleccion == null)
            {
                coleccion = new ColeccionDeArchivos();
                coleccion.Emit(new NuevaColeccionDeArchivosCreada(cmd.Firma, cmd.idColeccion));
            }
            else if (coleccion.YaTieneArchivo(cmd.Descriptor.Nombre))
                return ResultadoDelUpload.ResponderQueYaExiste();


            if (await this.fileWriter.TryWriteUnindexedIfNotExists(cmd.FileContent, cmd.idColeccion, cmd.Descriptor))
            {
                coleccion.Emit(new NuevoArchivoAgregadoALaColeccion(cmd.Firma, cmd.idColeccion, cmd.Descriptor));


                await this.repository.SaveAsync(coleccion);

                return ResultadoDelUpload.ResponderExitoso();
            }
            else
                return ResultadoDelUpload.ResponderQueYaExiste();
        }

        public async Task HandleAsync(EliminarArchivo cmd)
        {
            var coleccion = await this.repository.GetOrFailByIdAsync<ColeccionDeArchivos>(cmd.IdColeccion);

            if (!coleccion.YaTieneArchivo(cmd.NombreArchivo))
                throw new InvalidOperationException("El archivo ni siquiera existe");

            if (coleccion.EstaEliminado(cmd.NombreArchivo))
                throw new InvalidOperationException("El archivo ya esta eliminado");

            coleccion.Emit(new ArchivoEliminado(cmd.Firma, cmd.IdColeccion, cmd.NombreArchivo));

            await this.repository.SaveAsync(coleccion);
        }

        public async Task HandleAsync(RestaurarArchivo cmd)
        {
            var coleccion = await this.repository.GetOrFailByIdAsync<ColeccionDeArchivos>(cmd.IdColeccion);

            if (!coleccion.YaTieneArchivo(cmd.NombreArchivo))
                throw new InvalidOperationException("El archivo ni siquiera existe");

            if (!coleccion.EstaEliminado(cmd.NombreArchivo))
                throw new InvalidOperationException("El archivo no está eliminado, no necesita restaurarse");

            coleccion.Emit(new ArchivoRestaurado(cmd.Firma, cmd.IdColeccion, cmd.NombreArchivo));

            await this.repository.SaveAsync(coleccion);
        }
    }
}
