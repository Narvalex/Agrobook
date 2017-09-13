using Eventing;
using Eventing.Core.Serialization;
using Eventing.Log;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class FileWriter : IFileWriter
    {
        private readonly ILogLite log;
        private readonly IJsonSerializer serializer;
        private readonly string path;
        private readonly string unindexedPrefix = "unindexed-";

        public FileWriter(ILogLite log, IJsonSerializer serializer)
        {
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(log, nameof(log));

            this.serializer = serializer;
            this.log = log;
            this.path = Directory.GetCurrentDirectory() + @"\archivos";
            //this.path = @".\archivos";
        }

        public void CreateDirectoryIfNeeded()
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

        public void DeleteAllAndStartAgain()
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

            this.CreateDirectoryIfNeeded();
        }

        public async Task<bool> TryWriteUnindexedIfNotExists(HttpContent fileContent, string idColeccion, ArchivoDescriptor descriptor)
        {
            //var fileName = new string(fileContent.Headers.ContentDisposition.FileName.Trim().Where(c => c != '"').ToArray());
            var fileName = descriptor.Nombre;

            var coleccionPath = $"{this.path}\\{idColeccion}";
            if (!Directory.Exists(coleccionPath))
            {
                this.log.Info($"Creando el directorio para la nueva colección de archivos de {idColeccion}...");
                Directory.CreateDirectory(coleccionPath);
                this.log.Info($"Creacion exitosa del directorio {idColeccion}");
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
        public bool SetFileAsIndexedIfNeeded(string idColeccion, ArchivoDescriptor descriptor)
        {
            var fileName = descriptor.Nombre;
            var colectionPath = $"{this.path}\\{idColeccion}";

            var fullIndexedPath = $"{colectionPath}\\{fileName}";
            var fullUnindexedPath = $"{colectionPath}\\{this.unindexedPrefix}{fileName}";

            if (File.Exists(fullIndexedPath))
                return false;

            File.Move(fullUnindexedPath, fullIndexedPath);
            return true;
        }

        public FileStream GetFile(string idColeccion, string fileName)
        {
            var fileStream = new FileStream($"{this.path}\\{idColeccion}\\{fileName}", FileMode.Open, FileAccess.Read);
            return fileStream;
        }
    }
}
