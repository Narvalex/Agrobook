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

        public async Task<IList<ClienteDeApDto>> ObtenerClientes(string filtro)
        {
            return await this.QueryAsync(async context =>
            {
                var organizaciones = await context.Organizaciones.ToArrayAsync();
                var productores = await context.Usuarios.Where(x => x.EsProductor).ToArrayAsync();
                var orgDeUsuarios = await context.OrganizacionesDeUsuarios.ToArrayAsync();

                var clientesQuery =
                    organizaciones.Select(o => new ClienteDeApDto
                    {
                        Id = o.OrganizacionId,
                        Nombre = o.NombreParaMostrar,
                        Desc = "Organización",
                        Tipo = "org",
                        AvatarUrl = this.orgAvatarUrl
                    })
                    .Concat(productores.Select(p => new ClienteDeApDto
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

        public async Task<IList<ParcelaEntity>> ObtenerParcelas(string idProd)
            => await this.QueryAsync(async context =>
            await context.Parcelas.Where(x => x.IdProd == idProd).ToListAsync());

        public async Task<ParcelaEntity> ObtenerParcela(string idParcela)
            => await this.QueryAsync(async context =>
            await context.Parcelas.SingleAsync(x => x.Id == idParcela));

        public async Task<OrgConContratosDto[]> ObtenerOrgsConContratosDelProductor(string idProd)
            => await this.QueryAsync(async context =>
            (await context
                   .OrganizacionesDeUsuarios
                   .Where(x => x.UsuarioId == idProd)
                   .ToArrayAsync())
            .Select(x => new OrgConContratosDto
            {
                Org = new OrgDto { Id = x.OrganizacionId, Display = x.OrganizacionDisplay },
                Contratos = context
                            .Contratos
                            .Where(c => c.IdOrg == x.OrganizacionId)
                            .ToArray()
            })
            .ToArray());

        public async Task<ProdDto> GetProd(string idProd)
            => await this.QueryAsync(async context =>
                (await context.Usuarios.SingleAsync(x => x.Id == idProd))
                .Transform(u => new ProdDto
                {
                    Id = idProd,
                    Display = u.Display,
                    AvatarUrl = u.AvatarUrl,
                    Orgs = context.OrganizacionesDeUsuarios
                            .Where(x => x.UsuarioId == idProd)
                            .ToArray()
                            .Select(x => new OrgDto
                            {
                                Id = x.OrganizacionId,
                                Display = x.OrganizacionDisplay
                            })
                            .ToArray()
                }));

        public async Task<IList<ServicioEntity>> GetServiciosPorOrg(string idOrg)
            => await this.QueryAsync(async context =>
            await context.Servicios
            .Where(s => s.IdOrg == idOrg)
            .ToListAsync());

        public async Task<IList<ServicioEntity>> GetServiciosPorProd(string idProd)
            => await this.QueryAsync(async context =>
            await context.Servicios
            .Where(s => s.IdProd == idProd)
            .ToListAsync());

        public async Task<ServicioEntity> GetServicio(string idServicio)
            => await this.QueryAsync(async context =>
            await context.Servicios.SingleAsync(x => x.Id == idServicio));
    }
}
