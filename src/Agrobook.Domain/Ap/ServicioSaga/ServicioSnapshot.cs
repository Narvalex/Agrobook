using Eventing.Core.Domain;
using System;

namespace Agrobook.Domain.Ap.ServicioSaga
{
    public class ServicioSnapshot : Snapshot
    {
        public ServicioSnapshot(string streamName, int version, string idOrganizacion, string idContrato,
            DateTime fecha, bool eliminado, string idParcela)
            : base(streamName, version)
        {
            this.IdOrganizacion = idOrganizacion;
            this.IdContrato = idContrato;
            this.Fecha = fecha;
            this.EstaEliminado = eliminado;
            this.IdParcela = idParcela;
        }

        public string IdOrganizacion { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }
        public bool EstaEliminado { get; }
        public string IdParcela { get; }
    }
}
