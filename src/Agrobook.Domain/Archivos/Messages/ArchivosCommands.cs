using Agrobook.Domain.Common;
using System.Net.Http;

namespace Agrobook.Domain.Archivos
{
    public class AgregarArchivoAColeccion : MensajeAuditable
    {
        public AgregarArchivoAColeccion(Firma metadatos, string idProductor, ArchivoDescriptor archivo, HttpContent fileContent) : base(metadatos)
        {
            this.IdProductor = idProductor;
            this.Archivo = archivo;
            this.FileContent = fileContent;
        }

        public string IdProductor { get; }
        public ArchivoDescriptor Archivo { get; }
        public HttpContent FileContent { get; }
    }

    public class RegistrarDescargaExitosa : MensajeAuditable
    {
        public RegistrarDescargaExitosa(Firma metadatos, string productor, string nombreArchivo)
            : base(metadatos)
        {
            this.Productor = productor;
            this.NombreArchivo = nombreArchivo;
        }

        // Este es al que pertenece el archivo
        public string Productor { get; }
        public string NombreArchivo { get; }
    }
}
