using Agrobook.Core;
using Agrobook.Domain.Common;

namespace Agrobook.Domain.Archivos
{
    public class NuevaColeccionDeArchivosCreada : MensajeAuditable, IEvent
    {
        public NuevaColeccionDeArchivosCreada(Firma metadatos, string idColeccion) : base(metadatos)
        {
            this.IdColeccion = idColeccion;
        }

        public string IdColeccion { get; }

        public string StreamId => this.IdColeccion;
    }

    public class NuevoArchivoAgregadoALaColeccion : MensajeAuditable, IEvent
    {
        public NuevoArchivoAgregadoALaColeccion(Firma metadatos, string idColeccion, ArchivoDescriptor descriptor) : base(metadatos)
        {
            this.IdColeccion = idColeccion;
            this.Descriptor = descriptor;
        }

        public string IdColeccion { get; }
        public ArchivoDescriptor Descriptor { get; }
        public string StreamId => this.IdColeccion;
    }

    public class ArchivoDescargadoExitosamente : MensajeAuditable, IEvent
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

        public string StreamId => this.IdColeccion;
    }

    public class ArchivoEliminado : MensajeAuditable, IEvent
    {
        public ArchivoEliminado(Firma firma, string idColeccion, string nombreArchivo) : base(firma)
        {
            this.IdColeccion = idColeccion;
            this.NombreArchivo = nombreArchivo;
        }

        public string IdColeccion { get; }
        public string NombreArchivo { get; }

        public string StreamId => this.IdColeccion;
    }

    public class ArchivoRestaurado : MensajeAuditable, IEvent
    {
        public ArchivoRestaurado(Firma firma, string idColeccion, string nombreArchivo) : base(firma)
        {
            this.IdColeccion = idColeccion;
            this.NombreArchivo = nombreArchivo;
        }

        public string IdColeccion { get; }
        public string NombreArchivo { get; }

        public string StreamId => this.IdColeccion;
    }
}
