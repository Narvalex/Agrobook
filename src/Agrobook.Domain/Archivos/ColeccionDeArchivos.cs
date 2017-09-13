using Eventing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Archivos
{
    [StreamCategory("agrobook.coleccionesDeArchivos")]
    public class ColeccionDeArchivos : EventSourced
    {
        private IDictionary<string, int> filesWithSize = new Dictionary<string, int>();

        public ColeccionDeArchivos()
        {
            this.On<NuevaColeccionDeArchivosCreada>(e =>
            {
                this.SetStreamNameById(e.IdColeccion);
            });
            this.On<NuevoArchivoAgregadoALaColeccion>(e =>
            {
                this.filesWithSize.Add(e.Descriptor.Nombre, e.Descriptor.Size);
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
