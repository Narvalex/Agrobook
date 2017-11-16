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
        private string observaciones;

        public Servicio()
        {
            this.On<NuevoServicioRegistrado>(e =>
            {
                this.SetStreamNameById(e.StreamId);
                this.idOrganizacion = e.IdOrg;
                this.idContrato = e.IdContrato;
                this.IdProductor = e.IdProd;
                this.fecha = e.Fecha;
                this.observaciones = e.Observaciones;
            });
            this.On<DatosBasicosDelSevicioEditados>(e =>
            {
                this.idContrato = e.IdContrato;
                this.idOrganizacion = e.IdOrg;
                this.fecha = e.Fecha;
                this.observaciones = e.Observaciones;
            });
            this.On<ServicioEliminado>(e => this.EstaEliminado = true);
            this.On<ServicioRestaurado>(e => this.EstaEliminado = false);
            this.On<ParcelaDeServicioEspecificada>(e => this.IdParcela = e.IdParcela);
            this.On<ParcelaDeServicioCambiada>(e => this.IdParcela = e.IdParcela);
            this.On<PrecioDeServicioFijado>(e =>
            {
                this.TienePrecio = true;
                this.Precio = e.PrecioTotal;
            });
            this.On<PrecioDeServicioAjustado>(e =>
            {
                this.Precio = e.PrecioTotal;
            });
        }

        public bool TieneParcela => this.IdParcela != null;

        public string IdProductor { get; private set; }
        public string IdParcela { get; private set; } = null;
        public bool EstaEliminado { get; private set; } = false;

        public bool TienePrecio { get; private set; } = false;
        public decimal Precio { get; private set; } = default(decimal);

        public bool HayDiferenciaEnDatosBasicos(string idOrg, string idContrato, DateTime fecha, string observaciones)
        {
            if (idOrg != this.idOrganizacion) return true;
            if (idContrato != this.idContrato) return true;
            if (fecha != this.fecha) return true;
            if (observaciones != this.observaciones) return true;
            return false;
        }

        protected override ISnapshot TakeSnapshot()
            => new ServicioSnapshot(this.StreamName, this.Version, this.IdProductor, this.idOrganizacion,
                this.idContrato, this.fecha, this.observaciones, this.EstaEliminado, this.IdParcela);

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ServicioSnapshot)snapshot;
            this.IdProductor = state.IdProductor;
            this.idOrganizacion = state.IdOrganizacion;
            this.idContrato = state.IdContrato;
            this.fecha = state.Fecha;
            this.observaciones = state.Observaciones;
            this.EstaEliminado = state.EstaEliminado;
            this.IdParcela = state.IdParcela;
        }
    }

    public class ServicioSnapshot : Snapshot
    {
        public ServicioSnapshot(string streamName, int version, string idProductor, string idOrganizacion, string idContrato,
            DateTime fecha, string observaciones, bool eliminado, string idParcela)
            : base(streamName, version)
        {
            this.IdProductor = idProductor;
            this.IdOrganizacion = idOrganizacion;
            this.IdContrato = idContrato;
            this.Fecha = fecha;
            this.Observaciones = observaciones;
            this.EstaEliminado = eliminado;
            this.IdParcela = idParcela;
        }

        public string IdProductor { get; }
        public string IdOrganizacion { get; }
        public string IdContrato { get; }
        public DateTime Fecha { get; }
        public string Observaciones { get; }
        public bool EstaEliminado { get; }
        public string IdParcela { get; }
    }
}
