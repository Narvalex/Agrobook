using Agrobook.Domain.Common.ValueObjects;

namespace Agrobook.Domain.Ap.ValueObjects
{
    public class Parcela
    {
        public Parcela(string id, decimal hectareas, UbicacionDepartamental ubicacion, bool eliminada)
        {
            this.Id = id;
            this.Hectareas = hectareas;
            this.Ubicacion = ubicacion;
            this.Eliminada = eliminada;
        }

        public string Id { get; }
        public decimal Hectareas { get; private set; }
        public UbicacionDepartamental Ubicacion { get; }
        public bool Eliminada { get; private set; }

        // cmds
        internal void MarcarComoEliminada() => this.Eliminada = true;
        internal void MarcarComoRestaurada() => this.Eliminada = false;

        // querys
        internal bool EsDiferenteDe(Parcela parcela)
        {
            if (this.Id != parcela.Id) return true;
            if (this.Hectareas != parcela.Hectareas) return true;
            if (this.Ubicacion.IdDepartamento != parcela.Ubicacion.IdDepartamento) return true;
            if (this.Ubicacion.IdDistrito != parcela.Ubicacion.IdDistrito) return true;

            return false;
        }
    }
}
