using Agrobook.Core;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosService : EventSourcedService
    {
        private readonly IArchivosDelProductorFileManager fileWriter;

        public ArchivosService(IArchivosDelProductorFileManager fileWriter, IEventSourcedRepository repository, IDateTimeProvider dateTime) : base(repository, dateTime)
        {
            Ensure.NotNull(fileWriter, nameof(fileWriter));

            this.fileWriter = fileWriter;
        }

        public async Task<ResultadoDelUpload> HandleAsync(AgregarArchivoAColeccion cmd)
        {
            var coleccion = await this.repository.GetAsync<ColeccionDeArchivosDelProductor>(cmd.IdProductor);
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

    public class ResultadoDelUpload
    {
        public ResultadoDelUpload(bool exitoso, bool yaExiste)
        {
            this.Exitoso = exitoso;
            this.YaExiste = yaExiste;
        }

        public bool Exitoso { get; }
        public bool YaExiste { get; }

        public static ResultadoDelUpload ResponderExitoso() => new ResultadoDelUpload(true, false);
        public static ResultadoDelUpload ResponderQueYaExiste() => new ResultadoDelUpload(false, true);
    }
}
