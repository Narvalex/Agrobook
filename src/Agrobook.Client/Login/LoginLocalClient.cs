using Agrobook.Common;
using Agrobook.Domain.Usuarios;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public class LoginLocalClient : LocalClientBase, ILoginClient
    {
        private readonly IDateTimeProvider dateTime;
        private readonly UsuariosService usuariosService;

        public LoginLocalClient(IDateTimeProvider dateTime, UsuariosService usuariosService)
        {
            this.dateTime = dateTime;
            this.usuariosService = usuariosService;
        }

        public async Task<LoginResult> TryLoginAsync(string userName, string password)
        {
            var result = await this.usuariosService.HandleAsync(new IniciarSesion(userName, password, this.dateTime.Now));
            return result;
        }
    }
}
