using Agrobook.Domain.Common;
using Agrobook.Domain.DataWarehousing.Dimensions;
using Agrobook.Domain.Usuarios;
using Eventing.Core.Messaging;
using System;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing
{
    public class OrganizacionesEtl : EfDenormalizer<AgrobookDataWarehouseContext>,
        IHandler<NuevaOrganizacionCreada>,
        IHandler<NombreDeOrganizacionCambiado>
    {
        public OrganizacionesEtl(Func<AgrobookDataWarehouseContext> contextFactory) : base(contextFactory)
        {
        }

        public void Handle(long checkpoint, NuevaOrganizacionCreada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                context.OrganizacionDims.Add(new OrganizacionDim
                {
                    IdOrganizacion = e.Identificador,
                    Nombre = e.NombreParaMostrar
                });
            });
        }

        public void Handle(long checkpoint, NombreDeOrganizacionCambiado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var org = context.OrganizacionDims.Single(x => x.IdOrganizacion == e.IdOrg);
                org.Nombre = e.NombreNuevo;
            });
        }
    }
}
