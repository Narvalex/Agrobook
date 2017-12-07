using Agrobook.Domain.Common;
using Agrobook.Domain.DataWarehousing.DAOs.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.DataWarehousing.DAOs
{
    public class ApDao : DbContextQueryService<AgrobookDataWarehouseContext>
    {
        public ApDao(Func<AgrobookDataWarehouseContext> contextFactory) : base(contextFactory)
        {
        }

        public async Task<List<ServicioDeAp>> ObtenerServicios()
        {
            return await this.QueryAsync(async context =>
            {
                var list = await context.ServicioDeApFacts
                        .Where(x => x.Parcela != null
                        && !x.Eliminado)
                        .Include(x => x.Fecha)
                        .Include(x => x.Organizacion)
                        .Include(x => x.Contrato)
                        .Include(x => x.Productor)
                        .Include(x => x.Parcela)
                        .Include(x => x.ApPrecioPorHaServicio)
                        .Include(x => x.UsuarioQueRegistro)
                        .ToListAsync();

                return list.Select(s => new ServicioDeAp
                {
                    Año = s.Fecha.Año,
                    Contrato = s.Contrato.NombreContrato,
                    Fecha = new DateTime(s.Fecha.Año, s.Fecha.Mes, s.Fecha.Dia),
                    Organizacion = s.Organizacion.Nombre,
                    Parcela = s.Parcela.Nombre,
                    PrecioPorHa = s.ApPrecioPorHaServicio.Precio,
                    PrecioTotal = s.PrecioTotal,
                    Productor = s.Productor.Nombre,
                    Ha = s.Parcela.Hectareas

                })
                .ToList();
            });
        }
    }
}
