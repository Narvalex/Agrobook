using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Domain.Tests.Archivos
{
    public class TestableFileManager : IArchivosDelProductorFileManager
    {
        public bool RetornarResultadoExitoso { get; set; } = true;

        public void BorrarTodoYEmpezarDeNuevo()
        {
            throw new NotImplementedException();
        }

        public void CrearDirectoriosSiFaltan()
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
