using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Client.Archivos
{
    public class ArchivosClient : ClientBase
    {
        public ArchivosClient(HttpLite http, Func<string> tokenProvider = null) 
            : base(http, tokenProvider, "archivos")
        { }

        public async Task Upload(Stream fileStream, string fileName)
        {
            await base.Upload("upload", fileStream, fileName);
        }
    }
}
