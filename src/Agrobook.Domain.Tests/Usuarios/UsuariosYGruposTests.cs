using Agrobook.Core;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class UsuariosYGruposTests
    {
        private TestableEventSourcedService<UsuariosYGruposService> sut;

        public UsuariosYGruposTests()
        {
            this.sut = new TestableEventSourcedService<UsuariosYGruposService>(
                r => new UsuariosYGruposService(r));
        }

        #region ABM Usuarios
        [TestMethod]
        public void SePuedeCrearUsuarioNuevo()
        {
            this.sut.Given()
                    .When(s => s.Handle(new CrearNuevoUsuario(TestMeta.New, "Admin", "123")))
                    .Then(e =>
                    {
                        Assert.AreEqual(1, e.Count);
                        Assert.AreEqual("Admin", e.OfType<NuevoUsuarioCreado>().Single().Usuario);
                        Assert.AreEqual("123", e.OfType<NuevoUsuarioCreado>().Single().Password);
                    })
                    .And<Snapshot>(s =>
                    {
                        Assert.AreEqual("Admin", s.StreamName);
                    });
        }

        [TestMethod]
        public void CuandoElUsuarioYaExisteEntoncesNoSePuedeAgregarOtroIgual()
        {
            var usuario = "user1";
            this.sut.Given(usuario, new NuevoUsuarioCreado(TestMeta.New, "user1", "123"))
                    .When(s =>
                    {
                        Assert.ThrowsException<UniqueConstraintViolationException>(() =>
                        {
                            s.Handle(new CrearNuevoUsuario(TestMeta.New, "user1", "123"));
                        });
                    });
        }
        #endregion

        #region ABM Grupos
        [TestMethod]
        public void SePuedeCrearNuevoGrupo()
        {
            this.sut.Given()
                    .When(s => s.Handle(new CrearGrupo("nuevoGrupo", TestMeta.New)))
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
            this.sut.Given(streamName, new NuevoGrupoCreado(TestMeta.New, streamName))
                    .When(s =>
                    {
                        s.Handle(new AgregarUsuarioAGrupo(streamName, "user1", TestMeta.New));
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
        #endregion

        #region Login
        [TestMethod]
        public void NoSePuedeIniciarSesionDeUsuarioQueNoExiste()
        {
            var now = DateTime.Now;
            this.sut
                .Given()
                .When(s =>
                {
                    var result = s.Handle(new IniciarSesion("non-existent-user", "fakepass", now));

                    Assert.IsFalse(result.LoginExitoso);
                });
        }

        [TestMethod]
        public void SiElUsuarioExisteYLasCredencialesSonValidasSePuedeIniciarSesion()
        {
            var now = DateTime.Now;
            this.sut
                .Given("user1", new NuevoUsuarioCreado(TestMeta.New, "user1", "123"))
                .When(s =>
                {
                    var result = s.Handle(new IniciarSesion("user1", "123", now));

                    Assert.IsTrue(result.LoginExitoso);
                });
        }
        #endregion
    }
}
