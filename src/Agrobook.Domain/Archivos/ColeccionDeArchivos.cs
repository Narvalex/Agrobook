using Eventing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Archivos
{
    [StreamCategory("agrobook.coleccionesDeArchivos")]
    public class ColeccionDeArchivos : EventSourced
    {
        // int: size, bool: EstaEliminado
        private IDictionary<string, Tuple<int, bool>> filesWithSize = new Dictionary<string, Tuple<int, bool>>();

        public ColeccionDeArchivos()
        {
            this.On<NuevaColeccionDeArchivosCreada>(e =>
            {
                this.SetStreamNameById(e.IdColeccion);
            });
            this.On<NuevoArchivoAgregadoALaColeccion>(e =>
            {
                this.filesWithSize.Add(e.Descriptor.Nombre, new Tuple<int, bool>(e.Descriptor.Size, false));
            });
            this.On<ArchivoEliminado>(e =>
            {
                var size = this.filesWithSize[e.NombreArchivo].Item1;
                this.filesWithSize[e.NombreArchivo] = new Tuple<int, bool>(size, true);
            });
            this.On<ArchivoRestaurado>(e =>
            {
                var size = this.filesWithSize[e.NombreArchivo].Item1;
                this.filesWithSize[e.NombreArchivo] = new Tuple<int, bool>(size, false);
            });
        }

        public bool YaTieneArchivo(string nombreDelArchivo) => this.filesWithSize.ContainsKey(nombreDelArchivo);

        public int GetSize(string nombreArchivo) => this.filesWithSize[nombreArchivo].Item1;
        public bool EstaEliminado(string nombreArchivo) => this.filesWithSize[nombreArchivo].Item2;

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
        public ColeccionDeArchivosDelProductorSnapshot(string streamName, int version, KeyValuePair<string, Tuple<int, bool>>[] archivos) : base(streamName, version)
        {
            this.Archivos = archivos;
        }

        public KeyValuePair<string, Tuple<int, bool>>[] Archivos { get; }
    }
}
