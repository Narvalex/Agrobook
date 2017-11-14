using Eventing;
using Eventing.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Agrobook.Domain.Ap
{
    /// <summary>
    /// Un contrato de servicios profesionales de Agricultura de Precisión con una organización.
    /// Al contrato se le puede aplicar varias adendas, que extienden los términos del contrato.
    /// </summary>
    [StreamCategory("agrobook.ap.contratos")]
    public class Contrato : EventSourced
    {
        private readonly IDictionary<string, bool> adendasById = new Dictionary<string, bool>(); // if true = adenda eliminada

        public Contrato()
        {
            this.EstaEliminado = false;
            this.On<NuevoContrato>(e =>
            {
                this.SetStreamNameById(e.IdContrato);
                this.IdOrganizacion = e.IdOrganizacion;
            });
            this.On<NuevaAdenda>(e => this.adendasById.Add(e.IdAdenda, false));
            this.On<ContratoEliminado>(e => this.EstaEliminado = true);
            this.On<ContratoRestaurado>(e => this.EstaEliminado = false);
            this.On<AdendaEliminada>(e => this.adendasById[e.IdAdenda] = true);
            this.On<AdendaRestaurada>(e => this.adendasById[e.IdAdenda] = false);
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new ContratoSnapshot(this.StreamName, this.Version, this.IdOrganizacion, this.adendasById.ToArray(),
                this.EstaEliminado);
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ContratoSnapshot)snapshot;
            this.IdOrganizacion = state.IdOrganizacion;
            state.Adendas.ForEach(x => this.adendasById.Add(x.Key, x.Value));
            this.EstaEliminado = state.EstaEliminado;
        }

        public string IdOrganizacion { get; private set; }
        public bool TieneAdenda(string idAdenda) => this.adendasById.ContainsKey(idAdenda);
        public bool LaAdendaEstaEliminada(string idAdenda) => this.adendasById.ContainsKey(idAdenda) && this.adendasById[idAdenda];
        public bool EstaEliminado { get; private set; }
    }

    public class ContratoSnapshot : Snapshot
    {
        public ContratoSnapshot(string streamName, int version, string idOrganizacion, KeyValuePair<string, bool>[] adendas,
            bool estaEliminado)
            : base(streamName, version)
        {
            this.IdOrganizacion = idOrganizacion;
            this.Adendas = adendas;
            this.EstaEliminado = estaEliminado;
        }

        public string IdOrganizacion { get; }
        public KeyValuePair<string, bool>[] Adendas { get; }
        public bool EstaEliminado { get; }
    }
}
