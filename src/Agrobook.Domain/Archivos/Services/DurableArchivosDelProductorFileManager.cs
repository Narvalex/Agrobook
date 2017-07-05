using Agrobook.Core;
using Agrobook.Infrastructure.Log;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class DurableArchivosDelProductorFileManager : IArchivosDelProductorFileManager
    {
        private readonly ILogLite log;
        private readonly IJsonSerializer serializer;
        private readonly string path;
        private readonly string unindexedPrefix = "unindexed-";

        public DurableArchivosDelProductorFileManager(ILogLite log, IJsonSerializer serializer)
        {
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(log, nameof(log));

            this.serializer = serializer;
            this.log = log;
            this.path = Directory.GetCurrentDirectory() + @"\archivos";
            //this.path = @".\archivos";
        }

        public void CrearDirectoriosSiFaltan()
        {
            if (Directory.Exists(this.path))
            {
                this.log.Verbose($"El directorio {this.path} existe en el sistema");
                return;
            }

            this.log.Verbose($"Creando el directorio {this.path}...");
            Directory.CreateDirectory(this.path);
            this.log.Verbose($"El directorio {this.path} fué creado exitosamente");
        }

        public void BorrarTodoYEmpezarDeNuevo()
        {
            this.log.Warning($"Borrando todo los archivos en {this.path}");
            if (!Directory.Exists(this.path))
            {
                this.log.Warning($"El directorio {this.path} no existe. No se borró nada.");
            }
            else
            {
                Directory.Delete(this.path, true);
                this.log.Warning($"Fueron borrados todos los archivos en {this.path}");
            }

            this.CrearDirectoriosSiFaltan();
        }

        public async Task<bool> TryWriteUnindexedIfNotExists(HttpContent fileContent, string idProductor, ArchivoDescriptor metadatos)
        {
            //var fileName = new string(fileContent.Headers.ContentDisposition.FileName.Trim().Where(c => c != '"').ToArray());
            var fileName = $"{metadatos.Nombre}.{metadatos.Extension}";

            var coleccionPath = $"{this.path}\\{idProductor}";
            if (!Directory.Exists(coleccionPath))
            {
                this.log.Info($"Creando el directorio para la nueva colección de archivos de {idProductor}...");
                Directory.CreateDirectory(coleccionPath);
                this.log.Info($"Creacion exitosa del directorio {idProductor}");
            }

            using (var stream = await fileContent.ReadAsStreamAsync())
            {
                var fullIndexedPath = $"{coleccionPath}\\{fileName}";
                var fullUnindexedPath = $"{coleccionPath}\\{this.unindexedPrefix}{fileName}";

                if (File.Exists(fullIndexedPath) || File.Exists(fullUnindexedPath))
                    return false;

                using (var fileStream = File.Create(fullUnindexedPath))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    await stream.CopyToAsync(fileStream);
                }
            }

            return true;
        }

        // Returns false if not needed
        public bool SetFileAsIndexedIfNeeded(string idProductor, ArchivoDescriptor archivo)
        {
            var fileName = $"{archivo.Nombre}.{archivo.Extension}";
            var colectionPath = $"{this.path}\\{idProductor}";

            var fullIndexedPath = $"{colectionPath}\\{fileName}";
            var fullUnindexedPath = $"{colectionPath}\\{this.unindexedPrefix}{fileName}";

            if (File.Exists(fullIndexedPath))
                return false;

            File.Move(fullUnindexedPath, fullIndexedPath);
            return true;
        }

        public FileStream GetFile(string idProductor, string nombreArchivo, string extension)
        {
            var fileStream = new FileStream($"{this.path}\\{idProductor}\\{nombreArchivo}.{extension}", FileMode.Open, FileAccess.Read);
            return fileStream;
        }
    }
}
