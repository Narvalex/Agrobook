using Agrobook.Core;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public class ArchivosService : EventSourcedService
    {
        private readonly IJsonSerializer serializer;

        public ArchivosService(IEventSourcedRepository repository, IDateTimeProvider dateTime, IJsonSerializer serializer) : base(repository, dateTime)
        {
            Ensure.NotNull(serializer, nameof(serializer));

            this.serializer = serializer;
        }

        public async Task PersistirArchivoDelProductor(HttpContent content)
        {
            if (!content.IsMimeMultipartContent())
                throw new NotMimeMultipartException();

            var streamProvider = await content.ReadAsMultipartAsync();
            var fileContent = streamProvider.Contents.First();

            var metadatosSerializados = await streamProvider.Contents[1].ReadAsStringAsync();
            var metadatos = this.serializer.Deserialize<Metadatos>(metadatosSerializados);

            //var fileName = new string(fileContent.Headers.ContentDisposition.FileName.Trim().Where(c => c != '"').ToArray());
            var fileName = $"{metadatos.Nombre}.{metadatos.Extension}";

            using (var stream = await fileContent.ReadAsStreamAsync())
            {
                var formattedFileName = @"\" + fileName;

                var path = @"\files";
                var current = Directory.GetCurrentDirectory();
                var fullPath = current + path + formattedFileName;
                if (File.Exists(fullPath)) throw new FileAlreadyExistsException();
                using (var fileStream = File.Create(fullPath))
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.CopyTo(fileStream);
                }
            }
        }
    }

    public class Metadatos
    {
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public DateTime Fecha { get; set; }
        public string Desc { get; set; }
        // En Bytes
        public int Size { get; set; }
        public string IdProductor { get; set; }
    }

    public class NotMimeMultipartException : Exception
    {
        public NotMimeMultipartException()
        { }

        public NotMimeMultipartException(string message) : base(message)
        { }
    }

    public class FileAlreadyExistsException : Exception
    {
    }
}
