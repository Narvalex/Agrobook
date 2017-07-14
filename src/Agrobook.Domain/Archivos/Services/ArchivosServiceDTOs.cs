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
