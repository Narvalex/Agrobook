using Agrobook.Core;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class UsuariosYGruposTests
    {
        private TestableEventSourcedService<UsuariosYGruposService> sut;

        public UsuariosYGruposTests()
        {
            this.sut = new TestableEventSourcedService<UsuariosYGruposService>(
                r => new UsuariosYGruposService(r, new SimpleDateTimeProvider()));
        }

        #region ABM Usuarios
        [TestMethod]
        public void SiPuedeDetectarQueNoExisteTodaviaElUsuarioAdmin()
        {
            this.sut
                .Given()
                .When(s =>
                {
                    Assert.IsFalse(s.ExisteUsuarioAdmin);
                });
        }

        [TestMethod]
        public void SePuedeCrearElUsuarioAdminSiNoExiste()
        {
            this.sut
                .Given()
                .When(s =>
                {
                    s.CrearUsuarioAdminAsync();
                })
                .Then(e =>
                {
                    Assert.AreEqual(1, e.Count);
                    Assert.AreEqual("changeit", e.OfType<NuevoUsuarioCreado>().Single().Password);
                });
        }

        [TestMethod]
        public void SePuedeCrearUsuarioNuevo()
        {
            this.sut.Given()
                    .When(s => s.HandleAsync(new CrearNuevoUsuario(TestMeta.New, "Admin", "123")))
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
                            try
                            {
                                s.HandleAsync(new CrearNuevoUsuario(TestMeta.New, "user1", "123")).Wait();
                            }
                            catch (AggregateException ex)
                            {
                                throw ex.InnerException;
                            }
                        });
                    });
        }
        #endregion

        #region ABM Grupos
        [TestMethod]
        public void SePuedeCrearNuevoGrupo()
        {
            this.sut.Given()
                    .When(s => s.HandleAsync(new CrearGrupo("nuevoGrupo", TestMeta.New)).Wait())
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
        public async Task SePuedeAgregarUsuarioAUnGrupo()
        {
            var streamName = "grupoCreado";
            this.sut.Given(streamName, new NuevoGrupoCreado(TestMeta.New, streamName))
                    .When(async s =>
                    {
                        await s.HandleAsync(new AgregarUsuarioAGrupo(streamName, "user1", TestMeta.New));
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
        public async Task NoSePuedeIniciarSesionDeUsuarioQueNoExiste()
        {
            var now = DateTime.Now;
            this.sut
                .Given()
                .When(async s =>
                {
                    var result = await s.HandleAsync(new IniciarSesion("non-existent-user", "fakepass", now));

                    Assert.IsFalse(result.LoginExitoso);
                })
                .Then(e =>
                {
                    Assert.AreEqual(0, e.Count());
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
                    var result = s.HandleAsync(new IniciarSesion("user1", "123", now)).Result;

                    Assert.IsTrue(result.LoginExitoso);
                })
                .Then(e =>
                {
                    Assert.AreEqual(1, e.OfType<UsuarioInicioSesion>().Count());
                });
        }

        [TestMethod]
        public void SiElUsuarioExisteYLasCredencialesNoSonValidasEntoncesNoSePuedeIniciarSesion()
        {
            this.sut
                .Given("user1", new NuevoUsuarioCreado(TestMeta.New, "user1", "123"))
                .When(s =>
                {
                    var result = s.HandleAsync(new IniciarSesion("user1", "wrongPassword", DateTime.Now)).Result;

                    Assert.IsFalse(result.LoginExitoso);
                })
                .Then(e =>
                {
                    Assert.AreEqual(0, e.Count());
                });
        }
        #endregion
    }
}
