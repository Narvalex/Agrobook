namespace Agrobook.Domain.Ap.ValueObjects
{
    public class Parcela
    {
        public Parcela(string id, decimal hectareas, bool eliminada)
        {
            this.Id = id;
            this.Hectareas = hectareas;
            this.Eliminada = eliminada;
        }

        public string Id { get; }
        public decimal Hectareas { get; private set; }
        public bool Eliminada { get; private set; }

        internal void MarcarComoEliminada() => this.Eliminada = true;
        internal void MarcarComoRestaurada() => this.Eliminada = false;
    }
}
