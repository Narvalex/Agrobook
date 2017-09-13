using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;

namespace Agrobook.Domain.Tests.Archivos
{
    public class TestableFileManager : IFileWriter
    {
        public bool RetornarResultadoExitoso { get; set; } = true;

        public void DeleteAllAndStartAgain()
        {
            throw new NotImplementedException();
        }

        public void CreateDirectoryIfNeeded()
        {
            throw new NotImplementedException();
        }

        public FileStream GetFile(string idProductor, string nombreArchivo)
        {
            throw new NotImplementedException();
        }

        public bool SetFileAsIndexedIfNeeded(string idProductor, ArchivoDescriptor archivo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryWriteUnindexedIfNotExists(HttpContent fileContent, string idProductor, ArchivoDescriptor metadatos)
        {
            return Task.FromResult(this.RetornarResultadoExitoso);
        }
    }
}
