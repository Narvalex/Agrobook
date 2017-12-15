using Agrobook.Domain.Archivos.Services;
using Eventing.Client;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Agrobook.Client.Archivos
{
    public interface IArchivosQueryClient : ISecuredClient
    {
        Task<Stream> Download(string idColeccion, string nombreArchivo, string usuario);
        Task<IList<MetadatosDeArchivo>> ObtenerListaDeArchivos(string idColeccion);
        Task<Stream> Preview(string idColeccion, string nombreArchivo, string usuario);
    }
}