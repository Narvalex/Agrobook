using Agrobook.CLI.Common;
using Agrobook.Client.Ap;
using Agrobook.Client.Usuarios;
using Agrobook.Domain.Ap.Commands;
using Agrobook.Domain.Common;
using Agrobook.Domain.Usuarios.Login;
using System;
using System.Threading.Tasks;

namespace Agrobook.CLI.Controllers
{
    public class SeedController : CommonController
    {
        private readonly SeedView view;
        private readonly UsuariosClient usuariosClient;
        private readonly ApClient apClient;

        public SeedController(SeedView view, UsuariosClient usuariosClient, ApClient apClient)
            : base("seed")
        {
            this.view = view;
            this.usuariosClient = usuariosClient;
            this.apClient = apClient;
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
            // Usuarios
            var ealmiron = new UsuarioDto
            {
                NombreDeUsuario = "ealmiron",
                NombreParaMostrar = "Ernesto Almirón",
                AvatarUrl = "../assets/img/avatar/1.png",
                Claims = new string[] { ClaimDef.Roles.Tecnico },
                Password = "123"
            };
            await this.usuariosClient.CrearNuevoUsuario(ealmiron);
            this.view.NotificarUsuarioCreado(ealmiron);

            var jcordone = new UsuarioDto
            {
                NombreDeUsuario = "jcordone",
                NombreParaMostrar = "Jorge Cordone",
                AvatarUrl = "../assets/img/avatar/3.png",
                Claims = new string[] { ClaimDef.Roles.Gerente },
                Password = "123"
            };
            await this.usuariosClient.CrearNuevoUsuario(jcordone);
            this.view.NotificarUsuarioCreado(jcordone);

            var llang = new UsuarioDto
            {
                NombreDeUsuario = "llang",
                NombreParaMostrar = "Luciano Lang",
                AvatarUrl = "../assets/img/avatar/10.png",
                Claims = new string[] { ClaimDef.Roles.Productor },
                Password = "123"
            };
            await this.usuariosClient.CrearNuevoUsuario(llang);
            this.view.NotificarUsuarioCreado(llang);

            var cyamashita = new UsuarioDto
            {
                NombreDeUsuario = "cyamashita",
                NombreParaMostrar = "Carlos Yamashita",
                AvatarUrl = "../assets/img/avatar/10.png",
                Claims = new string[] { ClaimDef.Roles.Productor },
                Password = "123"
            };
            await this.usuariosClient.CrearNuevoUsuario(cyamashita);
            this.view.NotificarUsuarioCreado(cyamashita);


            // Orgs
            var nombreDeLaOrg = "Raúl Peña";
            var rpDto = await this.usuariosClient.CrearNuevaOrganización(nombreDeLaOrg);
            this.view.NotificarOrgCreada(nombreDeLaOrg, rpDto.Id);

            nombreDeLaOrg = "Pirapo";
            var pirapoDto = await this.usuariosClient.CrearNuevaOrganización(nombreDeLaOrg);
            this.view.NotificarOrgCreada(nombreDeLaOrg, pirapoDto.Id);

            nombreDeLaOrg = "Yguazu";
            var yguazuDto = await this.usuariosClient.CrearNuevaOrganización(nombreDeLaOrg);
            this.view.NotificarOrgCreada(nombreDeLaOrg, yguazuDto.Id);

            nombreDeLaOrg = "Friesland";
            var frieslandDto = await this.usuariosClient.CrearNuevaOrganización(nombreDeLaOrg);
            this.view.NotificarOrgCreada(nombreDeLaOrg, frieslandDto.Id);


            // Agregar usuarios a la organizacion
            await this.usuariosClient.AgregarUsuarioALaOrganizacion(llang.NombreDeUsuario, rpDto.Id);
            this.view.NotificarUsuarioAgregadoAOrg(llang.NombreParaMostrar, rpDto.Display);

            await this.usuariosClient.AgregarUsuarioALaOrganizacion(cyamashita.NombreDeUsuario, yguazuDto.Id);
            this.view.NotificarUsuarioAgregadoAOrg(cyamashita.NombreParaMostrar, yguazuDto.Display);


            var firma = new Firma("admin", DateTime.Now);
            // Contratos
            var cmdContratoRp = new RegistrarNuevoContrato(firma, rpDto.Id, "Contrato 2013", new DateTime(2013, 1, 1));
            var idContrato = await this.apClient.Send(cmdContratoRp);
            this.view.NotificarNuevoContrato(cmdContratoRp.NombreDelContrato, rpDto.Display);

            var cmdAdendaRp = new RegistrarNuevaAdenda(firma, idContrato, "Adenda 4", new DateTime(2014, 1, 1));
            var idAdenda = await this.apClient.Send(cmdAdendaRp);
            this.view.NotificarNuevaAdenda(cmdAdendaRp.NombreDeLaAdenda, rpDto.Display);
        }
    }
}
