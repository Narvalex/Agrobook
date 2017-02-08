using Agrobook.Core;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class GrupoDeUsuariosTests
    {
        private TestableEventSourcedService<UsuariosYGruposService> sut;

        public GrupoDeUsuariosTests()
        {
            this.sut = new TestableEventSourcedService<UsuariosYGruposService>(
                r => new UsuariosYGruposService(r));
        }

        [TestMethod]
        public void SePuedeCrearNuevoGrupo()
        {
            this.sut.Given()
                    .When(s => s.Handle(new CrearGrupo("nuevoGrupo")))
                    .Then(e =>
                    {
                        Assert.IsTrue(e.Count == 1);
                        Assert.IsTrue(e.OfType<NuevoGrupoCreado>().Single().IdGrupo == "nuevoGrupo");
                    })
                    .And<Snapshot>(s =>
                    {
                        Assert.IsTrue(s.StreamName == "nuevoGrupo");
                    });
        }

        [TestMethod]
        public void SePuedeAgregarUsuarioAUnGrupo()
        {
            var streamName = "grupoCreado";
            this.sut.Given(streamName, new NuevoGrupoCreado(streamName))
                    .When(s =>
                    {
                        s.Handle(new AgregarUsuarioAGrupo(streamName, "user1"));
                    })
                    .Then(e =>
                    {
                        Assert.IsTrue(e.Count == 1, "Cuenta de usuario");
                        Assert.IsTrue(e.OfType<UsuarioAgregadoAGrupo>().Single().IdGrupo == streamName, "Nombre del grupo");
                        Assert.IsTrue(e.OfType<UsuarioAgregadoAGrupo>().Single().IdUsuario == "user1", "Nombre del usuario");
                    })
                    .And<GrupoDeUsuariosSnapshot>(s =>
                    {
                        Assert.AreEqual(streamName, s.StreamName);
                        Assert.AreEqual(1, s.Usuarios.Length);
                        Assert.AreEqual("user1", s.Usuarios[0]);
                    });
        }
    }
}
