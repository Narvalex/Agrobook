using Agrobook.Core;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
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
                var usuarioAdmin = this.esReader.GetAsync<Usuario>(UsuariosConstants.UsuarioAdmin).Result;
                return usuarioAdmin != null;
            }
        }

        public bool EsProductor(string loginInfoEncriptado)
        {
            var loginInfo = this.cryptoSerializer.Deserialize<LoginInfo>(loginInfoEncriptado);

            var esProductor = loginInfo.Claims.Any(x => x == ClaimDef.Roles.Productor);
            return esProductor;
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
