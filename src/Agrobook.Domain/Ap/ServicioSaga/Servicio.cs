using Agrobook.Domain.Ap.Messages;
using Eventing.Core.Domain;
using System;

namespace Agrobook.Domain.Ap.ServicioSaga
{
    [StreamCategory("agrobook.ap.servicios")]
    public class Servicio : EventSourced
    {
        public string idOrganizacion;
        public string idContrato;
        public DateTime fecha;

        public Servicio()
        {
            this.On<NuevoServicioRegistrado>(e =>
            {
                this.SetStreamNameById(e.StreamId);
                this.idOrganizacion = e.IdOrg;
                this.idContrato = e.IdContrato;
                this.fecha = e.Fecha;
            });
            this.On<DatosBasicosDelSevicioEditados>(e =>
            {
                this.idContrato = e.IdContrato;
                this.idOrganizacion = e.IdOrg;
                this.fecha = e.Fecha;
            });
            this.On<ServicioEliminado>(e => this.EstaEliminado = true);
            this.On<ServicioRestaurado>(e => this.EstaEliminado = false);
        }

        public bool EstaEliminado { get; private set; } = false;

        protected override ISnapshot TakeSnapshot()
            => new ServicioSnapshot(this.StreamName, this.Version, this.idOrganizacion, this.idContrato, this.fecha, this.EstaEliminado);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ServicioSnapshot)snapshot;
            this.idOrganizacion = state.IdOrganizacion;
            this.idContrato = state.IdContrato;
            this.fecha = state.Fecha;
            this.EstaEliminado = state.EstaEliminado;
        }
    }
}
