using Agrobook.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Archivos
{
    [StreamCategory("agrobook.coleccionesDeArchivosDelProductor")]
    public class ColeccionDeArchivosDelProductor : EventSourced
    {
        private List<string> archivos = new List<string>();

        public ColeccionDeArchivosDelProductor()
        {
            this.On<NuevaColeccionDeArchivosDelProductorCreada>(e =>
            {
                this.StreamName = e.IdProductor;
            });
            this.On<NuevoArchivoAgregadoALaColeccion>(e =>
            {
                this.archivos.Add(e.Archivo.Nombre);
            });
        }

        public bool YaTieneArchivo(string nombreDelArchivo) => this.archivos.Any(a => a == nombreDelArchivo);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ColeccionDeArchivosDelProductorSnapshot)snapshot;
            this.archivos.AddRange(state.Archivos);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new ColeccionDeArchivosDelProductorSnapshot(this.StreamName, this.Version, this.archivos.ToArray());
        }
    }

    public class ColeccionDeArchivosDelProductorSnapshot : Snapshot
    {
        public ColeccionDeArchivosDelProductorSnapshot(string streamName, int version, string[] archivos) : base(streamName, version)
        {
            this.Archivos = archivos;
        }

        public string[] Archivos { get; }
    }

    public class ArchivoDescriptor
    {
        public ArchivoDescriptor(string nombre, string extension, DateTime fecha, string descripcion, int size)
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
