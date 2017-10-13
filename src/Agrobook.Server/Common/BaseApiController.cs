using Agrobook.Domain.Usuarios;
using System.Linq;
using System.Web.Http;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Server
{
    public abstract class BaseApiController : ApiController
    {
        protected readonly UsuariosService usuariosService = ServiceLocator.ResolveSingleton<UsuariosService>();

        public bool SolicitaSoloSusPropiosDatos(string usuarioAConsultar)
        {
            var claims = this.usuariosService.GetClaims(this.ActionContext.GetToken());
            if (!claims.Any(x => x == Roles.Admin || x == Roles.Gerente || x == Roles.Tecnico)
                && this.usuariosService.GetCurrentUser(this.ActionContext.GetToken()).Usuario != usuarioAConsultar)
                return false;

            return true;
        }
    }
}
