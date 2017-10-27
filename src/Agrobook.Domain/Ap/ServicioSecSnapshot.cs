using Eventing.Core.Domain;

namespace Agrobook.Domain.Ap
{
    public class ServicioSecSnapshot : Snapshot
    {
        public ServicioSecSnapshot(string streamName, int version, int ultimoNroDeServicioDelProductor) : base(streamName, version)
        {
            this.UltimoNroDeServicioDelProductor = ultimoNroDeServicioDelProductor;
        }

        public int UltimoNroDeServicioDelProductor { get; }
    }
}
