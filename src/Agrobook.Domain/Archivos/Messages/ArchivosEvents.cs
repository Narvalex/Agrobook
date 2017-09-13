using Agrobook.Domain.Common;

namespace Agrobook.Domain.Archivos
{
    public class NuevaColeccionDeArchivosCreada : MensajeAuditable
    {
        public NuevaColeccionDeArchivosCreada(Firma metadatos, string idColeccion) : base(metadatos)
        {
            this.IdColeccion = idColeccion;
        }

        public string IdColeccion { get; }
    }

    public class NuevoArchivoAgregadoALaColeccion : MensajeAuditable
    {
        public NuevoArchivoAgregadoALaColeccion(Firma metadatos, string idColeccion, ArchivoDescriptor descriptor) : base(metadatos)
        {
            this.IdColeccion = idColeccion;
            this.Descriptor = descriptor;
        }

        public string IdColeccion { get; }
        public ArchivoDescriptor Descriptor { get; }
    }

    public class ArchivoDescargadoExitosamente : MensajeAuditable
    {
        public ArchivoDescargadoExitosamente(Firma metadatos, string idColeccion, string nombreArchivo, int size) : base(metadatos)
        {
            this.IdColeccion = idColeccion;
            this.NombreArchivo = nombreArchivo;
            this.Size = size;
        }

        public string IdColeccion { get; }
        public string NombreArchivo { get; }
        public int Size { get; }
    }
}
