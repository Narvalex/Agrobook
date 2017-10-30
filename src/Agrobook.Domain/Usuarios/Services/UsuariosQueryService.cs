using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using Eventing;
using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Usuarios.Services
{
    public class UsuariosQueryService : AgrobookQueryService
    {
        private readonly IJsonSerializer cryptoSerializer;

        public UsuariosQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader reader, IJsonSerializer crytoSerializer)
            : base(contextFactory, reader)
        {
            Ensure.NotNull(crytoSerializer, nameof(crytoSerializer));

            this.cryptoSerializer = crytoSerializer;
        }

        public bool ExisteUsuarioAdmin
        {
            get
            {
                var usuarioAdmin = this.esReader.GetByIdAsync<Usuario>(UsuariosConstants.UsuarioAdmin).Result;
                return usuarioAdmin != null;
            }
        }

        public string[] ObtenerClaims(string loginInfoEncriptado)
        {
            var loginInfo = this.cryptoSerializer.Deserialize<LoginInfo>(loginInfoEncriptado);

            return loginInfo.Claims;
        }

        public async Task<IList<UsuarioInfoBasica>> ObtenerTodosLosUsuarios()
        {
            return await this.QueryAsync(async context =>
            {
                var lista = await context.Usuarios.Select(u => new UsuarioInfoBasica
                {
                    Nombre = u.Id,
                    NombreParaMostrar = u.Display,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();

                return lista;
            });
        }

        public async Task<IList<UsuarioInfoBasica>> ObtenerTodosLosUsuariosMenosAdmines()
        {
            return await this.QueryAsync(async context =>
            {
                var lista = await context.Usuarios
                .Where(x => !x.EsAdmin)
                .Select(u => new UsuarioInfoBasica
                {
                    Nombre = u.Id,
                    NombreParaMostrar = u.Display,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();

                return lista;
            });
        }

        public async Task<IList<UsuarioInfoBasica>> ObtenerTodosLosProductores()
        {
            return await this.QueryAsync(async context =>
            {
                var lista = await context.Usuarios
                .Where(x => !x.EsAdmin && !x.EsGerente && !x.EsTecnico && x.EsProductor)
                .Select(u => new UsuarioInfoBasica
                {
                    Nombre = u.Id,
                    NombreParaMostrar = u.Display,
                    AvatarUrl = u.AvatarUrl
                })
                .ToListAsync();

                return lista;
            });
        }

        public async Task<UsuarioInfoBasica> ObtenerUsuarioInfoBasica(string usuario)
        {
            return await this.QueryAsync(async context =>
            {
                var dto = await context
                                .Usuarios
                                .Where(u => u.Id == usuario)
                                .Select(u => new UsuarioInfoBasica
                                {
                                    Nombre = u.Id,
                                    NombreParaMostrar = u.Display,
                                    AvatarUrl = u.AvatarUrl
                                })
                                .SingleOrDefaultAsync();

                return dto;
            });
        }
    }
}
