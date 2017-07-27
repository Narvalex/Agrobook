using Agrobook.Common;
using Eventing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Archivos
{
    [StreamCategory("agrobook.coleccionesDeArchivosDelProductor")]
    public class ColeccionDeArchivosDelProductor : EventSourced
    {
        private IDictionary<string, int> filesWithSize = new Dictionary<string, int>();

        public ColeccionDeArchivosDelProductor()
        {
            this.On<NuevaColeccionDeArchivosDelProductorCreada>(e =>
            {
                this.StreamName = e.IdProductor;
            });
            this.On<NuevoArchivoAgregadoALaColeccion>(e =>
            {
                this.filesWithSize.Add(e.Archivo.Nombre, e.Archivo.Size);
            });
        }

        public bool YaTieneArchivo(string nombreDelArchivo) => this.filesWithSize.ContainsKey(nombreDelArchivo);

        public int GetSize(string nombreArchivo) => this.filesWithSize[nombreArchivo];

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ColeccionDeArchivosDelProductorSnapshot)snapshot;
            foreach (var item in state.Archivos)
                this.filesWithSize.Add(item);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new ColeccionDeArchivosDelProductorSnapshot(this.StreamName, this.Version, this.filesWithSize.ToArray());
        }
    }

    public class ColeccionDeArchivosDelProductorSnapshot : Snapshot
    {
        public ColeccionDeArchivosDelProductorSnapshot(string streamName, int version, KeyValuePair<string, int>[] archivos) : base(streamName, version)
        {
            this.Archivos = archivos;
        }

        public KeyValuePair<string, int>[] Archivos { get; }
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
