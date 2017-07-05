using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public interface IArchivosDelProductorFileManager
    {
        void CrearDirectoriosSiFaltan();
        void BorrarTodoYEmpezarDeNuevo();
        Task<bool> TryWriteUnindexedIfNotExists(HttpContent fileContent, string idProductor, ArchivoDescriptor metadatos);
        bool SetFileAsIndexedIfNeeded(string idProductor, ArchivoDescriptor archivo);
        FileStream GetFile(string idProductor, string nombreArchivo, string extension);
    }
}
