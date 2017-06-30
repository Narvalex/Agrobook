using System;

namespace Agrobook.Domain.Archivos.Services
{
    public class MetadatosDelArchivo
    {
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public DateTime Fecha { get; set; }
        public string Desc { get; set; }
        // En Bytes
        public int Size { get; set; }
        public string IdProductor { get; set; }
    }
}
