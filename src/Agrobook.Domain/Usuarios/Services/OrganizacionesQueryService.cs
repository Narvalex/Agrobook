using Agrobook.Common;
using Agrobook.Domain.Common;
using Eventing.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class OrganizacionesQueryService : AgrobookQueryService
    {
        public OrganizacionesQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader esReader) : base(contextFactory, esReader)
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
                                    Display = o.NombreParaMostrar
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
                                 .Select(o => new OrganizacionDto
                                 {
                                     Id = o.OrganizacionId,
                                     Display = o.OrganizacionDisplay
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
                                 .Select(o => new OrganizacionDto
                                 {
                                     Id = o.OrganizacionId,
                                     Display = o.OrganizacionDisplay,
                                     UsuarioEsMiembro = true
                                 })
                                 .ToListAsync();

                var orgsRestantes = await context
                                 .Organizaciones
                                 .Select(o => new OrganizacionDto
                                 {
                                     Id = o.OrganizacionId,
                                     Display = o.NombreParaMostrar,
                                     UsuarioEsMiembro = false
                                 })
                                 .ToListAsync();

                var orgRestantesFiltrado = orgsRestantes.Where(o => !orgsDelUsuario.Any(ou => ou.Id == o.Id));

                orgsDelUsuario.AddRange(orgRestantesFiltrado);

                return orgsDelUsuario;
            });
        }


        public async Task<IList<GrupoDto>> ObtenerGrupos(string idOrganizacion, string idUsuario)
        {
            return await this.QueryAsync(async context =>
            {
                var todosLosGruposDeLaOrg = await context
                                .Grupos
                                .Where(g => g.OrganizacionId == idOrganizacion)
                                .ToListAsync();

                var gruposDelUsuario = await context
                                                .GruposDeUsuarios
                                                .Where(x => x.OrganizacionId == idOrganizacion && x.UsuarioId == idUsuario)
                                                .ToListAsync();

                var lista = todosLosGruposDeLaOrg.Select(x =>
                            new GrupoDto
                            {
                                Id = x.Id,
                                Display = x.Display,
                                UsuarioEsMiembro = gruposDelUsuario.Any(g => g.GrupoId == x.Id)
                            })
                            .ToList();

                return lista;
            });
        }
    }
}
