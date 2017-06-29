using Agrobook.Domain.Common;

namespace Agrobook.Domain.Archivos
{
    public class AgregarArchivoAColeccion : MensajeAuditable
    {
        public AgregarArchivoAColeccion(Metadatos metadatos, string idProductor, Archivo archivo) : base(metadatos)
        {
            this.IdProductor = idProductor;
            this.Archivo = archivo;
        }

        public string IdProductor { get; }
        public Archivo Archivo { get; }
    }
}
