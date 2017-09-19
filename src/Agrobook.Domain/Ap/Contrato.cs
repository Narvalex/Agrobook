using Agrobook.Domain.Ap.Messages;
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
    [StreamCategory("agrobook.contratos")]
    public class Contrato : EventSourced
    {
        private readonly IDictionary<string, object> adendasById = new Dictionary<string, object>();

        public Contrato()
        {
            this.On<NuevoContrato>(e =>
            {
                this.SetStreamNameById(e.IdContrato);
                this.IdOrganizacion = e.IdOrganizacion;
            });
            this.On<NuevaAdenda>(e => this.adendasById.Add(e.IdAdenda, null));
        }

        public string IdOrganizacion { get; private set; }

        protected override ISnapshot TakeSnapshot()
        {
            return new ContratoSnapshot(this.StreamName, this.Version, this.IdOrganizacion, this.adendasById.Keys.ToArray());
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ContratoSnapshot)snapshot;
            this.IdOrganizacion = state.IdOrganizacion;
            state.Adendas.ForEach(x => this.adendasById.Add(x, null));
        }

        public bool TieneAdenda(string idAdenda) => this.adendasById.ContainsKey(idAdenda);
    }

    public class ContratoSnapshot : Snapshot
    {
        public ContratoSnapshot(string streamName, int version, string idOrganizacion, string[] adendas) : base(streamName, version)
        {
            this.IdOrganizacion = idOrganizacion;
            this.Adendas = adendas;
        }

        public string IdOrganizacion { get; }
        public string[] Adendas { get; }
    }
}
