using Agrobook.Domain.Common;
using Eventing.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap.Services
{
    public class ApQueryService : AgrobookQueryService
    {
        private readonly string orgAvatarUrl = "./assets/img/avatar/org-icon.png";

        public ApQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader esReader) : base(contextFactory, esReader)
        { }

        public async Task<IList<ClienteDeAp>> ObtenerClientes(string filtro)
        {
            return await this.QueryAsync(async context =>
            {
                var organizaciones = await context.Organizaciones.ToArrayAsync();
                var productores = await context.Usuarios.Where(x => x.EsProductor).ToArrayAsync();
                var orgDeUsuarios = await context.OrganizacionesDeUsuarios.ToArrayAsync();

                var clientesQuery =
                    organizaciones.Select(o => new ClienteDeAp
                    {
                        Id = o.OrganizacionId,
                        Nombre = o.NombreParaMostrar,
                        Desc = "Organización",
                        Tipo = "org",
                        AvatarUrl = this.orgAvatarUrl
                    })
                    .Concat(productores.Select(p => new ClienteDeAp
                    {
                        Id = p.Id,
                        Nombre = p.Display,
                        Tipo = "prod",
                        AvatarUrl = p.AvatarUrl,
                        Desc = orgDeUsuarios
                                .Where(o => o.UsuarioId == p.Id)
                                .Select(o => organizaciones.First(x => x.OrganizacionId == o.OrganizacionId).NombreParaMostrar)
                                .Aggregate("",
                                (acumulate, orgName) =>
                                    acumulate == "" ? orgName : $"{acumulate}, {orgName}")
                    }));

                switch (filtro)
                {
                    case "todos":
                        return clientesQuery.ToList();
                    case "prod":
                        return clientesQuery.Where(x => x.Tipo == "prod").ToList();
                    case "org":
                        return clientesQuery.Where(x => x.Tipo == "org").ToList();
                    default:
                        throw new ArgumentException("El filtro es inválido", nameof(filtro));
                }
            });
        }

        public async Task<OrgDto> ObtenerOrg(string idOrg)
        {
            return await this.QueryAsync(async context =>
            {
                var entity = await context.Organizaciones
                .SingleAsync(x => x.OrganizacionId == idOrg);

                return new OrgDto
                {
                    Id = idOrg,
                    Display = entity.NombreParaMostrar,
                    AvatarUrl = this.orgAvatarUrl
                };
            });
        }

        public async Task<IList<ContratoEntity>> ObtenerContratos(string idOrg)
            => await this.QueryAsync(async context =>
                await context.Contratos.Where(x => x.IdOrg == idOrg).ToListAsync());
    }
}
