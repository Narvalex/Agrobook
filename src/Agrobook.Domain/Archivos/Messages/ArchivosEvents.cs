using Agrobook.Domain.Common;

namespace Agrobook.Domain.Archivos
{
    public class NuevaColeccionDeArchivosDelProductorCreada : MensajeAuditable
    {
        public NuevaColeccionDeArchivosDelProductorCreada(Firma metadatos, string idProductor) : base(metadatos)
        {
            this.IdProductor = idProductor;
        }

        public string IdProductor { get; }
    }

    public class NuevoArchivoAgregadoALaColeccion : MensajeAuditable
    {
        public NuevoArchivoAgregadoALaColeccion(Firma metadatos, string idProductor, ArchivoDescriptor archivo) : base(metadatos)
        {
            this.IdProductor = idProductor;
            this.Archivo = archivo;
        }

        public string IdProductor { get; }
        public ArchivoDescriptor Archivo { get; }
    }

    public class ArchivoDescargadoExitosamente : MensajeAuditable
    {
        public ArchivoDescargadoExitosamente(Firma metadatos, string productor, string nombreArchivo, int size) : base(metadatos)
        {
            this.Productor = productor;
            this.NombreArchivo = nombreArchivo;
            this.Size = size;
        }

        public string Productor { get; }
        public string NombreArchivo { get; }
        public int Size { get; }
    }
}
