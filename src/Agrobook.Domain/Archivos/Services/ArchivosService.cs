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
        private readonly IArchivosDelProductorFileManager fileWriter;
        private readonly ConcurrentDictionary<string, SemaphoreSlim> locks = new ConcurrentDictionary<string, SemaphoreSlim>();

        public ArchivosService(IArchivosDelProductorFileManager fileWriter, IEventSourcedRepository repository) : base(repository)
        {
            Ensure.NotNull(fileWriter, nameof(fileWriter));

            this.fileWriter = fileWriter;
        }

        public async Task<ResultadoDelUpload> HandleAsync(AgregarArchivoAColeccion cmd)
        {
            // ToDo: improve
            var @lock = this.locks.GetOrAdd(cmd.IdProductor, new SemaphoreSlim(1, 1));
            await @lock.WaitAsync();
            try
            {
                // This could be so much optimized
                var resultado = await HandleAsyncWithPesimisticConcurrencyLock(cmd);
                return resultado;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                @lock.Release();
            }
        }

        public async Task HandleAsync(RegistrarDescargaExitosa cmd)
        {
            var coleccion = await this.repository.GetOrFailByIdAsync<ColeccionDeArchivosDelProductor>(cmd.Productor);
            coleccion.Emit(new ArchivoDescargadoExitosamente(cmd.Metadatos, cmd.Productor, cmd.NombreArchivo, coleccion.GetSize(cmd.NombreArchivo)));

            await this.repository.SaveAsync(coleccion);
        }

        private async Task<ResultadoDelUpload> HandleAsyncWithPesimisticConcurrencyLock(AgregarArchivoAColeccion cmd)
        {
            var coleccion = await this.repository.GetByIdAsync<ColeccionDeArchivosDelProductor>(cmd.IdProductor);
            if (coleccion == null)
            {
                coleccion = new ColeccionDeArchivosDelProductor();
                coleccion.Emit(new NuevaColeccionDeArchivosDelProductorCreada(cmd.Metadatos, cmd.IdProductor));
            }
            else if (coleccion.YaTieneArchivo(cmd.Archivo.Nombre))
                return ResultadoDelUpload.ResponderQueYaExiste();


            if (await this.fileWriter.TryWriteUnindexedIfNotExists(cmd.FileContent, cmd.IdProductor, cmd.Archivo))
            {
                coleccion.Emit(new NuevoArchivoAgregadoALaColeccion(cmd.Metadatos, cmd.IdProductor, cmd.Archivo));


                await this.repository.SaveAsync(coleccion);

                return ResultadoDelUpload.ResponderExitoso();
            }
            else
                return ResultadoDelUpload.ResponderQueYaExiste();
        }
    }
}
