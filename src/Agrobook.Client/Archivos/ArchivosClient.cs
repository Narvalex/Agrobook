using Agrobook.Domain.Archivos.Services;
using Eventing.Client.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Archivos
{
    public class ArchivosClient : ClientBase
    {
        public ArchivosClient(HttpLite http, Func<string> tokenProvider = null)
            : base(http, tokenProvider, "archivos")
        { }

        public async Task<ResultadoDelUpload> Upload(Stream fileStream, string fileName, string metadatosSerializados)
        {
            var dto = await base.Upload<ResultadoDelUpload>("upload", fileStream, fileName, metadatosSerializados);
            return dto;
        }
    }
}
