using Agrobook.Core;

namespace Agrobook.Domain.Usuarios
{
    [StreamCategory("agrobook.organizaciones")]
    public class Organizacion : EventSourced
    {
        public Organizacion()
        {
            this.On<NuevaOrganizacionCreada>(e =>
            {
                this.StreamName = e.Identificador;
                this.Nombre = e.Identificador;
                this.NombreParaMostrar = e.NombreParaMostrar;
            });
        }

        public string Nombre { get; private set; }
        public string NombreParaMostrar { get; private set; }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (OrganizacionSnapshot)snapshot;
            this.Nombre = state.Nombre;
            this.NombreParaMostrar = state.NombreParaMostrar;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new OrganizacionSnapshot(this.StreamName, this.Version, this.Nombre, this.NombreParaMostrar);
        }
    }
}
