using Agrobook.Domain.Ap.Commands;
using Eventing;
using Eventing.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Ap
{
    [StreamCategory("agrobook.ap.productores")]
    public class Productor : EventSourced
    {
        private readonly IDictionary<string, bool> parcelasById = new Dictionary<string, bool>(); // if true: deleted

        public Productor()
        {
            this.On<NuevoProductorRegistrado>(e => this.SetStreamNameById(e.IdProductor));
            this.On<NuevaParcelaRegistrada>(e => this.parcelasById.Add(e.IdParcela, false));
            this.On<ParcelaEliminada>(e => this.parcelasById[e.IdParcela] = true);
            this.On<ParcelaRestaurada>(e => this.parcelasById[e.IdParcela] = false);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new ProductorSnapshot(this.StreamName, this.Version, this.parcelasById.ToArray());
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ProductorSnapshot)snapshot;
            state.Parcelas.ForEach(x => this.parcelasById.Add(x.Key, x.Value));
        }

        public bool TieneParcela(string idParcela) => this.parcelasById.ContainsKey(idParcela);
        public bool ParcelaEstaEliminada(string idParcela) => this.parcelasById.ContainsKey(idParcela) && this.parcelasById[idParcela];
    }
}
