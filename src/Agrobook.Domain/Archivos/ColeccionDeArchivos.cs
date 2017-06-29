using Agrobook.Core;
using System;

namespace Agrobook.Domain.Archivos
{
    [StreamCategory("agrobook.coleccionesDeArchivos")]
    public class ColeccionDeArchivos : EventSourced
    {
        public ColeccionDeArchivos()
        {

        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return base.TakeSnapshot();
        }
    }

    public class Archivo
    {
        public Archivo(string nombre, string extension, DateTime fecha, string descripcion, int size)
        {
            this.Nombre = nombre;
            this.Extension = extension;
            this.Fecha = fecha;
            this.Descripcion = descripcion;
            this.Size = size;
        }

        public string Nombre { get; }
        public string Extension { get; }
        public DateTime Fecha { get; }
        public string Descripcion { get; }
        public int Size { get; }
    }
}
