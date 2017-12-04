using Agrobook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesQueryService : DbContextQueryService<AgrobookDbContext>
    {
        public OrganizacionesQueryService(Func<AgrobookDbContext> contextFactory) : base(contextFactory)
        { }

        public async Task<IList<OrganizacionDto>> ObtenerOrganizaciones()
        {
            return await this.QueryAsync(async context =>
            {
                var dto = await context
                                .Organizaciones
                                .Select(o => new OrganizacionDto
                                {
                                    Id = o.OrganizacionId,
                                    Display = o.NombreParaMostrar,
                                    Deleted = o.EstaEliminada
                                })
                                .ToListAsync();
                return dto;
            });
        }

        public async Task<IList<OrganizacionDto>> ObtenerOrganizaciones(string idUsuario)
        {
            return await this.QueryAsync(async context =>
            {
                var list = await context
                                 .OrganizacionesDeUsuarios
                                 .Where(o => o.UsuarioId == idUsuario)
                                 .Join(context.Organizaciones, outer => outer.OrganizacionId, inner => inner.OrganizacionId,
                                 (orgDeU, o) => new { orgDeU = orgDeU, o = o })
                                 .Select(joined => new OrganizacionDto
                                 {
                                     Id = joined.o.OrganizacionId,
                                     Display = joined.orgDeU.OrganizacionDisplay,
                                     Deleted = joined.o.EstaEliminada
                                 })
                                 .ToListAsync();
                return list;
            });
        }

        public async Task<IList<OrganizacionDto>> ObtenerOrganizacionesMarcadasDelUsuario(string idUsuario)
        {
            return await this.QueryAsync(async context =>
            {
                var orgsDelUsuario = await context
                                 .OrganizacionesDeUsuarios
                                 .Where(o => o.UsuarioId == idUsuario)
                                 .Join(context.Organizaciones, inner => inner.OrganizacionId, outer => outer.OrganizacionId,
                                 (inner, outer) => new { orgDeU = inner, org = outer })
                                 .Select(joined => new OrganizacionDto
                                 {
                                     Id = joined.org.OrganizacionId,
                                     Display = joined.orgDeU.OrganizacionDisplay,
                                     UsuarioEsMiembro = true,
                                     Deleted = joined.org.EstaEliminada
                                 })
                                 .ToListAsync();

                var orgsRestantes = await context
                                 .Organizaciones
                                 .Select(o => new OrganizacionDto
                                 {
                                     Id = o.OrganizacionId,
                                     Display = o.NombreParaMostrar,
                                     UsuarioEsMiembro = false,
                                     Deleted = o.EstaEliminada
                                 })
                                 .ToListAsync();

                var orgRestantesFiltrado = orgsRestantes.Where(o => !orgsDelUsuario.Any(ou => ou.Id == o.Id));

                orgsDelUsuario.AddRange(orgRestantesFiltrado);

                return orgsDelUsuario;
            });
        }
    }
}
