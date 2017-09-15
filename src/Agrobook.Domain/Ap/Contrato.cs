using Agrobook.Domain.Ap.Messages;
using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap
{
    /// <summary>
    /// Un contrato de servicios profesionales de Agricultura de Precisión con una organización.
    /// Al contrato se le puede aplicar varias adendas, que extienden los términos del contrato.
    /// </summary>
    [StreamCategory("agrobook.contratos")]
    public class Contrato : EventSourced
    {
        public Contrato()
        {
            this.On<NuevoContrato>(e =>
            {
                this.SetStreamNameById(e.IdContrato);
                this.IdOrganizacion = e.IdOrganizacion;
            });
        }

        public string IdOrganizacion { get; private set; }

        protected override ISnapshot TakeSnapshot()
        {
            return new ContratoSnapshot(this.StreamName, this.Version, this.IdOrganizacion);
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            var state = (ContratoSnapshot)snapshot;
            this.IdOrganizacion = state.IdOrganizacion;
        }
    }

    public class ContratoSnapshot : Snapshot
    {
        public ContratoSnapshot(string streamName, int version, string idOrganizacion) : base(streamName, version)
        {
            this.IdOrganizacion = idOrganizacion;
        }

        public string IdOrganizacion { get; }
    }
}
