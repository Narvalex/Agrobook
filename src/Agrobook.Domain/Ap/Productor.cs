using Agrobook.Domain.Ap.ValueObjects;
using Eventing.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Ap
{
    [StreamCategory("agrobook.ap.productores")]
    public class Productor : EventSourced
    {
        private readonly List<Parcela> parcelas = new List<Parcela>();

        public Productor()
        {
            this.On<NuevoProductorRegistrado>(e => this.SetStreamNameById(e.IdProductor));
            this.On<NuevaParcelaRegistrada>(e => this.parcelas.Add(new Parcela(e.IdParcela, e.Hectareas, e.Ubicacion, false)));
            this.On<ParcelaEditada>(e =>
            {
                var parcela = this.parcelas.Single(x => x.Id == e.IdParcela);
                this.parcelas.Remove(parcela);
                this.parcelas.Add(new Parcela(e.IdParcela, e.Hectareas, e.Ubicacion, parcela.Eliminada));
            });
            this.On<ParcelaEliminada>(e => this.parcelas.Single(x => x.Id == e.IdParcela).MarcarComoEliminada());
            this.On<ParcelaRestaurada>(e => this.parcelas.Single(x => x.Id == e.IdParcela).MarcarComoRestaurada());
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new ProductorSnapshot(this.StreamName, this.Version, this.parcelas.ToArray());
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ProductorSnapshot)snapshot;
            this.parcelas.AddRange(state.Parcelas);
        }

        public bool TieneParcela(string idParcela) => this.parcelas.Any(x => x.Id == idParcela);

        // Le hacemos una copia
        public Parcela MirarParcela(string idParcela)
            => this.parcelas
                .Where(x => x.Id == idParcela)
                .Select(x => new Parcela(x.Id, x.Hectareas, x.Ubicacion, x.Eliminada))
                .Single();

        public bool ParcelaEstaEliminada(string idParcela) => this.parcelas.Single(x => x.Id == idParcela).Eliminada;
    }

    public class ProductorSnapshot : Snapshot
    {
        public ProductorSnapshot(string streamName, int version, Parcela[] parcelas) : base(streamName, version)
        {
            this.Parcelas = parcelas;
        }

        public Parcela[] Parcelas { get; }
    }
}
