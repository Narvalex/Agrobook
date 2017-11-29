using Agrobook.Domain.Ap;
using Agrobook.Domain.Common;
using Agrobook.Domain.DataWarehousing.Dimensions;
using Agrobook.Domain.DataWarehousing.Facts;
using Eventing.Core.Messaging;
using System;
using System.Linq;

namespace Agrobook.Domain.DataWarehousing.ETLs
{
    public class ApServiciosEtl : EfDenormalizer<AgrobookDataWarehouseContext>,
        IHandler<NuevoServicioRegistrado>,
        IHandler<DatosBasicosDelSevicioEditados>
    {
        public ApServiciosEtl(Func<AgrobookDataWarehouseContext> contextFactory) : base(contextFactory)
        {
        }

        public void Handle(long checkpoint, NuevoServicioRegistrado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var idTiempo = TiempoDim.GetIdTiempoFromDateTime(e.Fecha);
                var tiempo = context.TiempoDims.SingleOrDefault(x => x.IdTiempo == idTiempo);
                if (tiempo == null)
                {
                    tiempo = TiempoDim.New(e.Fecha);
                    context.TiempoDims.Add(tiempo);
                }


                context.ServicioDeApFacts.Add(new ServicioDeApFact
                {
                    IdServicio = e.IdServicio,
                    Productor = context.UsuarioDims.Single(x => x.IdUsuario == e.IdProd),
                    Organizacion = context.OrganizacionDims.Single(x => x.IdOrganizacion == e.IdOrg),
                    Contrato = context.ContratoDims.Single(x => x.IdContrato == e.IdContrato),
                    UsuarioQueRegistro = context.UsuarioDims.Single(x => x.IdUsuario == e.Firma.Usuario),
                    Fecha = tiempo
                });
            });
        }

        public void Handle(long checkpoint, DatosBasicosDelSevicioEditados e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var idTiempo = TiempoDim.GetIdTiempoFromDateTime(e.Fecha);
                var tiempo = context.TiempoDims.SingleOrDefault(x => x.IdTiempo == idTiempo);
                if (tiempo == null)
                {
                    tiempo = TiempoDim.New(e.Fecha);
                    context.TiempoDims.Add(tiempo);
                }

                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);
                servicio.Organizacion = context.OrganizacionDims.Single(x => x.IdOrganizacion == e.IdOrg);
                servicio.Contrato = context.ContratoDims.Single(x => x.IdContrato == e.IdContrato);
                servicio.UsuarioQueRegistro = context.UsuarioDims.Single(x => x.IdUsuario == e.Firma.Usuario);
                servicio.Fecha = tiempo;
            });
        }
    }
}
