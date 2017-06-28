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

        public async Task Upload(Stream fileStream, string fileName, string metadatos)
        {
            await base.Upload("upload", fileStream, fileName, metadatos);
        }
    }
}
