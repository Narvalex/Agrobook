using System;

namespace Agrobook.Domain.Archivos
{
    public class ArchivoDescriptor
    {
        public ArchivoDescriptor(string nombre, string extension, DateTime fecha, string tipo, int size)
        {
            this.Nombre = nombre;
            this.Extension = extension;
            this.Fecha = fecha;
            this.Tipo = tipo;
            this.Size = size;
        }

        public string Nombre { get; }
        public string Extension { get; }
        public string Tipo { get; }
        public DateTime Fecha { get; }
        // En Byte
        public int Size { get; }
    }
}
