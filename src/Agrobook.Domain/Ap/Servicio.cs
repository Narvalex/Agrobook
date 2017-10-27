using Agrobook.Domain.Ap.Messages;
using Eventing.Core.Domain;
using System;

namespace Agrobook.Domain.Ap
{
    [StreamCategory("agrobook.ap.servicios")]
    public class Servicio : EventSourced
    {
        private string idOrganizacion;
        private string idContrato;
        private DateTime fecha;

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
            this.On<ParcelaDeServicioEspecificada>(e => this.IdParcela = e.IdParcela);
            this.On<ParcelaDeServicioCambiada>(e => this.IdParcela = e.IdParcela);
        }

        public bool EstaEliminado { get; private set; } = false;
        public string IdParcela { get; private set; } = null;

        public bool HayDiferenciaEnDatosBasicos(string idOrg, string idContrato, DateTime fecha)
        {
            if (idOrg != this.idOrganizacion) return true;
            if (idContrato != this.idContrato) return true;
            if (fecha != this.fecha) return true;
            return false;
        }

        protected override ISnapshot TakeSnapshot()
            => new ServicioSnapshot(this.StreamName, this.Version, this.idOrganizacion, this.idContrato, this.fecha, this.EstaEliminado,
                this.IdParcela);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ServicioSnapshot)snapshot;
            this.idOrganizacion = state.IdOrganizacion;
            this.idContrato = state.IdContrato;
            this.fecha = state.Fecha;
            this.EstaEliminado = state.EstaEliminado;
            this.IdParcela = state.IdParcela;
        }
    }
}
