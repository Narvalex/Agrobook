using Agrobook.Domain.Common;
using System.Net.Http;

namespace Agrobook.Domain.Archivos
{
    public class AgregarArchivoAColeccion : MensajeAuditable
    {
        public AgregarArchivoAColeccion(Metadatos metadatos, string idProductor, ArchivoDescriptor archivo, HttpContent fileContent) : base(metadatos)
        {
            this.IdProductor = idProductor;
            this.Archivo = archivo;
            this.FileContent = fileContent;
        }

        public string IdProductor { get; }
        public ArchivoDescriptor Archivo { get; }
        public HttpContent FileContent { get; }
    }
}
