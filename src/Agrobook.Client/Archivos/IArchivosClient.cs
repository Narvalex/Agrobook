using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using Eventing.Client;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Archivos
{
    public interface IArchivosClient : ISecuredClient
    {
        Task EliminarArchivo(EliminarArchivo cmd);
        Task RestaurarArchivo(RestaurarArchivo cmd);
        Task<ResultadoDelUpload> Upload(Stream fileStream, string fileName, string metadatosSerializados);
    }
}