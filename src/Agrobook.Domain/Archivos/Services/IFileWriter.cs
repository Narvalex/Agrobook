using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Agrobook.Domain.Archivos.Services
{
    public interface IFileWriter
    {
        void CreateDirectoryIfNeeded();
        void DeleteAllAndStartAgain();
        Task<bool> TryWriteUnindexedIfNotExists(HttpContent fileContent, string idColeccion, ArchivoDescriptor metadatos);
        bool SetFileAsIndexedIfNeeded(string idColeccion, ArchivoDescriptor archivo);
        FileStream GetFile(string idProductor, string fileName);
    }
}
