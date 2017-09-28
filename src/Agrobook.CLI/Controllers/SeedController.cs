using Agrobook.CLI.Common;
using Agrobook.Client.Usuarios;
using Agrobook.Domain.Usuarios.Login;
using System.Threading.Tasks;

namespace Agrobook.CLI.Controllers
{
    public class SeedController : CommonController
    {
        private readonly SeedView view;
        private readonly UsuariosClient usuariosClient;

        public SeedController(SeedView view, UsuariosClient usuariosClient)
            : base("seed")
        {
            this.view = view;
            this.usuariosClient = usuariosClient;
        }

        public override string CommandDescription => "Seed: Esto ayuda a llenar el sistema de datos de muestra";

        public void StartSeedCommandLoop()
        {
            this.view.ShowWellcomeScreen();
            var peticion = this.view.PedirIniciarOCancelarSeed();
            if (!peticion.Equals("s", System.StringComparison.InvariantCultureIgnoreCase))
            {
                this.view.MostrarQueSeSaleDelSeed();
                return;
            }

            this.view.MostrarQueElProcesoSeedSeInicio();
            this.Seed().Wait();
            this.view.MostrarQueElProcesoSeedFinalizo();
        }

        private async Task Seed()
        {
            var usuarioDto = new UsuarioDto
            {
                NombreDeUsuario = "prod",
                NombreParaMostrar = "ProductorUno",
                AvatarUrl = "./assets/img/avatar/10.png",
                Claims = new string[] { ClaimDef.Roles.Productor },
                Password = "123"
            };
            await this.usuariosClient.CrearNuevoUsuario(usuarioDto);
            this.view.NotificarUsuarioCreado(usuarioDto);

            var nombreDeLaOrg = "Cooperativa Chortitzer";
            var orgDto = await this.usuariosClient.CrearNuevaOrganización(nombreDeLaOrg);
            this.view.NotificarOrgCreada(nombreDeLaOrg);

            await this.usuariosClient.AgregarUsuarioALaOrganizacion(usuarioDto.NombreDeUsuario, orgDto.Id);
            this.view.NotificarUsuarioAgregadoAOrg();
        }
    }
}
