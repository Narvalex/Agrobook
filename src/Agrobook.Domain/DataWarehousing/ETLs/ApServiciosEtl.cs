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
        IHandler<DatosBasicosDelSevicioEditados>,
        IHandler<ServicioEliminado>,
        IHandler<ServicioRestaurado>,
        IHandler<ParcelaDeServicioEspecificada>,
        IHandler<ParcelaDeServicioCambiada>,
        IHandler<PrecioDeServicioFijado>,
        IHandler<PrecioDeServicioAjustado>
    {
        public ApServiciosEtl(Func<AgrobookDataWarehouseContext> contextFactory) : base(contextFactory)
        {
        }

        public void Handle(long checkpoint, NuevoServicioRegistrado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var tiempo = TiempoDim.GetOrAddTiempo(e.Fecha, context.TiempoDims);

                context.ServicioDeApFacts.Add(new ServicioDeApFact
                {
                    IdServicio = e.IdServicio,
                    Productor = context.UsuarioDims.Single(x => x.IdUsuario == e.IdProd),
                    Organizacion = context.OrganizacionDims.Single(x => x.IdOrganizacion == e.IdOrg),
                    Contrato = context.ContratoDims.Single(x => x.IdContrato == e.IdContrato),
                    UsuarioQueRegistro = context.UsuarioDims.Single(x => x.IdUsuario == e.Firma.Usuario),
                    Eliminado = false,
                    Fecha = tiempo
                });
            });
        }

        public void Handle(long checkpoint, DatosBasicosDelSevicioEditados e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var tiempo = TiempoDim.GetOrAddTiempo(e.Fecha, context.TiempoDims);

                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);
                servicio.Organizacion = context.OrganizacionDims.Single(x => x.IdOrganizacion == e.IdOrg);
                servicio.Contrato = context.ContratoDims.Single(x => x.IdContrato == e.IdContrato);
                servicio.UsuarioQueRegistro = context.UsuarioDims.Single(x => x.IdUsuario == e.Firma.Usuario);
                servicio.Fecha = tiempo;
            });
        }

        public void Handle(long checkpoint, ServicioEliminado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);
                servicio.Eliminado = true;
            });
        }

        public void Handle(long checkpoint, ServicioRestaurado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);
                servicio.Eliminado = false;
            });
        }

        public void Handle(long checkpoint, ParcelaDeServicioEspecificada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);

                servicio.Parcela = context.ParcelaDims.Single(x => x.IdParcela == e.IdParcela);
            });
        }

        public void Handle(long checkpoint, ParcelaDeServicioCambiada e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);

                servicio.Parcela = context.ParcelaDims.Single(x => x.IdParcela == e.IdParcela);
            });
        }

        public void Handle(long checkpoint, PrecioDeServicioFijado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);

                servicio.ApPrecioPorHaServicio = ApPrecioPorHaServicioDim.GetOrAdd(e.PrecioTotal, servicio.Parcela.Hectareas, context.PrecioPorHaServicioApDims);
                servicio.PrecioTotal = e.PrecioTotal;
            });
        }

        public void Handle(long checkpoint, PrecioDeServicioAjustado e)
        {
            this.Denormalize(checkpoint, context =>
            {
                var servicio = context.ServicioDeApFacts.Single(x => x.IdServicio == e.IdServicio);

                servicio.ApPrecioPorHaServicio = ApPrecioPorHaServicioDim.GetOrAdd(e.PrecioTotal, servicio.Parcela.Hectareas, context.PrecioPorHaServicioApDims);
                servicio.PrecioTotal = e.PrecioTotal;
            });
        }
    }
}
