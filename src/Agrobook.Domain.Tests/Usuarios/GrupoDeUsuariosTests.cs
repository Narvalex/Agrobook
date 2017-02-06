using Agrobook.Domain.Usuarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class GrupoDeUsuariosTests
    {
        private readonly GrupoDeUsuarios sut;

        public GrupoDeUsuariosTests()
        {
            this.sut = new GrupoDeUsuarios();
        }

        [TestMethod]
        public void SePuedeCrearNuevoGrupo()
        {
            // When
            var cmd = new CrearGrupo("grupoDeEjemplo");
            this.sut.Handle(cmd);

            // Then
            Assert.IsTrue(this.sut.NewEvents.OfType<NuevoGrupoCreado>().Count() == 1);

            var e = this.sut.NewEvents.OfType<NuevoGrupoCreado>().Single();
            Assert.IsTrue(e.IdGrupo == cmd.IdGrupo);
        }
    }
}
