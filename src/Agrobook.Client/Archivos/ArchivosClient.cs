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

        // Los metadatos estan serializados de la forma simple { prop1: '', prop2: ''}
        //public async Task Upload(Stream fileStream, string fileName, string metadatosSerializados)
        //{
        //    await base.Upload("upload", fileStream, fileName, metadatosSerializados);
        //}

        public async Task<ResultadoDelUpload> Upload(Stream fileStream, string fileName, string metadatosSerializados)
        {
            var dto = await base.Upload<ResultadoDelUpload>("upload", fileStream, fileName, metadatosSerializados);
            return dto;
        }

        public async Task<ResultadoDelUpload> UploadV2(Stream fileStream, string fileName, string metadatosSerializados)
        {
            var dto = await base.Upload<ResultadoDelUpload>("upload/v2", fileStream, fileName, metadatosSerializados);
            return dto;
        }
    }
}
