using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap
{
    public class NumeracionDeServiciosSnapshot : Snapshot
    {
        public NumeracionDeServiciosSnapshot(string streamName, int version, int ultimoNroDeServicioDelProductor) : base(streamName, version)
        {
            this.UltimoNroDeServicioDelProductor = ultimoNroDeServicioDelProductor;
        }

        public int UltimoNroDeServicioDelProductor { get; }
    }
}
