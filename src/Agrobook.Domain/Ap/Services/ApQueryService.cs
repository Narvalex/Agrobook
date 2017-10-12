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
        private readonly string orgAvatarUrl = "../assets/img/avatar/org-icon.png";

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

        public async Task<IList<ServicioDto>> GetServiciosPorOrg(string idOrg)
           => await this.QueryAsync(async context =>
                 await context.Servicios
                    .Where(x => x.IdOrg == idOrg && x.IdParcela != null)
                    .Join(context.Organizaciones, s => s.IdOrg, o => o.OrganizacionId,
                     (s, o) => new { serv = s, org = o })
                    .Join(context.Contratos, servOrg => servOrg.serv.IdContrato, c => c.Id,
                     (so, c) => new { so = so, c = c })
                    .Join(context.Parcelas, soc => soc.so.serv.IdParcela, p => p.Id,
                     (soc, p) => new { soc = soc, p = p })
                    .Join(context.Usuarios, socp => socp.soc.so.serv.IdProd, u => u.Id,
                     (socp, u) =>
                     new ServicioDto
                     {
                         ContratoDisplay = socp.soc.c.Display,
                         Eliminado = socp.soc.so.serv.Eliminado,
                         Fecha = socp.soc.so.serv.Fecha,
                         Id = socp.soc.so.serv.Id,
                         IdContrato = socp.soc.so.serv.IdContrato,
                         IdOrg = socp.soc.so.serv.IdOrg,
                         IdProd = socp.soc.so.serv.IdProd,
                         ProdDisplay = u.Display,
                         OrgDisplay = socp.soc.so.org.NombreParaMostrar,
                         ParcelaId = socp.soc.so.serv.IdParcela,
                         ParcelaDisplay = socp.p.Display
                     })
                    .Union(
                     context.Servicios
                    .Where(x => x.IdOrg == idOrg && x.IdParcela == null)
                    .Join(context.Organizaciones, s => s.IdOrg, o => o.OrganizacionId,
                     (s, o) => new { serv = s, org = o })
                     .Join(context.Usuarios, orgServ => orgServ.serv.IdProd, usuarios => usuarios.Id,
                     (so, u) => new { servOrg = so, usuarios = u })
                    .Join(context.Contratos, sou => sou.servOrg.serv.IdContrato, c => c.Id,
                     (sou, c) => new ServicioDto
                     {
                         ContratoDisplay = c.Display,
                         Eliminado = sou.servOrg.serv.Eliminado,
                         Fecha = sou.servOrg.serv.Fecha,
                         Id = sou.servOrg.serv.Id,
                         IdContrato = sou.servOrg.serv.IdContrato,
                         IdOrg = sou.servOrg.serv.IdOrg,
                         IdProd = sou.servOrg.serv.IdProd,
                         ProdDisplay = sou.usuarios.Display,
                         OrgDisplay = sou.servOrg.org.NombreParaMostrar,
                         ParcelaId = null,
                         ParcelaDisplay = null
                     }))
                    .ToListAsync());


        public async Task<IList<ServicioDto>> GetServiciosPorProd(string idProd)
            => await this.QueryAsync(async context =>
                 await context.Servicios
                    .Where(x => x.IdProd == idProd && x.IdParcela != null)
                    .Join(context.Organizaciones, s => s.IdOrg, o => o.OrganizacionId,
                     (s, o) => new { serv = s, org = o })
                    .Join(context.Contratos, servOrg => servOrg.serv.IdContrato, c => c.Id,
                     (so, c) => new { so = so, c = c })
                    .Join(context.Parcelas, soc => soc.so.serv.IdParcela, p => p.Id,
                     (soc, p) => new ServicioDto
                     {
                         ContratoDisplay = soc.c.Display,
                         Eliminado = soc.so.serv.Eliminado,
                         Fecha = soc.so.serv.Fecha,
                         Id = soc.so.serv.Id,
                         IdContrato = soc.so.serv.IdContrato,
                         EsAdenda = soc.c.EsAdenda,
                         IdContratoDeLaAdenda = soc.c.IdContratoDeLaAdenda,
                         IdOrg = soc.so.serv.IdOrg,
                         IdProd = soc.so.serv.IdProd,
                         OrgDisplay = soc.so.org.NombreParaMostrar,
                         ParcelaId = soc.so.serv.IdParcela,
                         ParcelaDisplay = p.Display
                     })
                    .Union(
                     context.Servicios
                    .Where(x => x.IdProd == idProd && x.IdParcela == null)
                    .Join(context.Organizaciones, s => s.IdOrg, o => o.OrganizacionId,
                     (s, o) => new { serv = s, org = o })
                    .Join(context.Contratos, servOrg => servOrg.serv.IdContrato, c => c.Id,
                     (so, c) => new ServicioDto
                     {
                         ContratoDisplay = c.Display,
                         Eliminado = so.serv.Eliminado,
                         Fecha = so.serv.Fecha,
                         Id = so.serv.Id,
                         IdContrato = so.serv.IdContrato,
                         EsAdenda = c.EsAdenda,
                         IdContratoDeLaAdenda = c.IdContratoDeLaAdenda,
                         IdOrg = so.serv.IdOrg,
                         IdProd = so.serv.IdProd,
                         OrgDisplay = so.org.NombreParaMostrar,
                         ParcelaId = null,
                         ParcelaDisplay = null
                     }))
                    .ToListAsync());

        public async Task<ServicioDto> GetServicio(string idServicio)
           => await (await this.QueryAsync(async context =>
                 (await context.Servicios
                    .Where(x => x.Id == idServicio)
                    .Join(context.Organizaciones,
                    s => s.IdOrg, o => o.OrganizacionId, (s, o) => new { serv = s, org = o })
                    .Join(context.Contratos, servOrg => servOrg.serv.IdContrato, c => c.Id,
                     (so, c) => new ServicioDto
                     {
                         ContratoDisplay = c.Display,
                         Eliminado = so.serv.Eliminado,
                         Fecha = so.serv.Fecha,
                         Id = so.serv.Id,
                         IdContrato = so.serv.IdContrato,
                         EsAdenda = c.EsAdenda,
                         IdContratoDeLaAdenda = c.IdContratoDeLaAdenda,
                         IdOrg = so.serv.IdOrg,
                         IdProd = so.serv.IdProd,
                         OrgDisplay = so.org.NombreParaMostrar,
                         ParcelaId = so.serv.IdParcela
                     })
                    .SingleAsync())
                    .Transform(async x =>
                    {
                        if (x.ParcelaId is null)
                            return x;
                        x.ParcelaDisplay = (await context.Parcelas.SingleAsync(p => p.Id == x.ParcelaId)).Display;
                        return x;
                    })));

    }
}
