using Agrobook.Domain.Common;
using System.Net.Http;

namespace Agrobook.Domain.Archivos
{
    public class AgregarArchivoAColeccion : MensajeAuditable
    {
        public AgregarArchivoAColeccion(Firma metadatos, string idColeccion, ArchivoDescriptor descriptor, HttpContent fileContent) : base(metadatos)
        {
            this.idColeccion = idColeccion;
            this.Descriptor = descriptor;
            this.FileContent = fileContent;
        }

        public string idColeccion { get; }
        public ArchivoDescriptor Descriptor { get; }
        public HttpContent FileContent { get; }
    }

    public class RegistrarDescargaExitosa : MensajeAuditable
    {
        public RegistrarDescargaExitosa(Firma metadatos, string idColeccion, string nombreArchivo)
            : base(metadatos)
        {
            this.IdColeccion = idColeccion;
            this.NombreArchivo = nombreArchivo;
        }

        // Este es al que pertenece el archivo
        public string IdColeccion { get; }
        public string NombreArchivo { get; }
    }

    public class EliminarArchivo : MensajeAuditable
    {
        public EliminarArchivo(Firma metadatos, string idColeccion, string nombreArchivo)
            : base(metadatos)
        {
            this.IdColeccion = idColeccion;
            this.NombreArchivo = nombreArchivo;
        }

        // Este es al que pertenece el archivo
        public string IdColeccion { get; }
        public string NombreArchivo { get; }
    }

    public class RestaurarArchivo : MensajeAuditable
    {
        public RestaurarArchivo(Firma metadatos, string idColeccion, string nombreArchivo)
            : base(metadatos)
        {
            this.IdColeccion = idColeccion;
            this.NombreArchivo = nombreArchivo;
        }

        // Este es al que pertenece el archivo
        public string IdColeccion { get; }
        public string NombreArchivo { get; }
    }
}
