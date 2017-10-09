using System;

namespace Agrobook.Domain.Archivos.Services
{
    public class MetadatosDeArchivo
    {
        public string Nombre { get; set; }
        public string Extension { get; set; }
        public string Tipo { get; set; }
        public DateTime Fecha { get; set; }
        // En Bytes
        public int Size { get; set; }
        public string IdColeccion { get; set; }
        public bool Deleted { get; set; }
    }

    public class ResultadoDelUpload
    {
        public ResultadoDelUpload(bool exitoso, bool yaExiste)
        {
            this.Exitoso = exitoso;
            this.YaExiste = yaExiste;
        }

        public bool Exitoso { get; }
        public bool YaExiste { get; }

        public static ResultadoDelUpload ResponderExitoso() => new ResultadoDelUpload(true, false);
        public static ResultadoDelUpload ResponderQueYaExiste() => new ResultadoDelUpload(false, true);
    }
}
