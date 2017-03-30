using Agrobook.Core;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Login;
using Agrobook.Infrastructure.Cryptography;
using Agrobook.Infrastructure.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class UsuariosTests
    {
        private TestableEventSourcedService<UsuariosService> sut;
        private CryptoSerializer crypto;

        public UsuariosTests()
        {
            this.crypto = new CryptoSerializer(new FauxCrypto());
            this.sut = new TestableEventSourcedService<UsuariosService>(
                r => new UsuariosService(r, new SimpleDateTimeProvider(), this.crypto));
        }

        [TestMethod]
        public void ElNombreDelStreamSeGeneraCorrectamente()
        {
            var usuario = new Usuario();
            usuario.Emit(new NuevoUsuarioCreado(TestMeta.New, "user", "Us Er", "avatar.png", "encriptedata"));

            Assert.AreEqual("agrobook.usuarios-user", usuario.StreamName);
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
                    s.CrearUsuarioAdminAsync().Wait();
                })
                .Then(e =>
                {
                    Assert.AreEqual(1, e.Count);
                    Assert.AreEqual("admin", e.OfType<NuevoUsuarioCreado>().Single().Usuario);

                    var loginInfo = this.crypto.Deserialize<LoginInfo>(e.OfType<NuevoUsuarioCreado>().Single().LoginInfoEncriptado);

                    Assert.AreEqual("changeit", loginInfo.Password);
                });
        }

        [TestMethod]
        public void SePuedeDetectarQueSiExisteElUsuarioAdminUnaVezCreado()
        {
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestMeta.New, "admin", "admin", "*.png", "password"))
                .When(s =>
                {
                    Assert.IsTrue(s.ExisteUsuarioAdmin);
                });
        }

        [TestMethod]
        public void SePuedeCrearUsuarioNuevoSinNingunPermiso()
        {
            var avatarUrl = "app/avatar.png";
            this.sut.Given()
                    .When(s =>
                        s.HandleAsync(new CrearNuevoUsuario(TestMeta.New, "user1", "User One", avatarUrl, "123", null)).Wait())
                    .Then(e =>
                    {
                        var ev = e.OfType<NuevoUsuarioCreado>().Single();
                        Assert.AreEqual(1, e.Count);
                        Assert.AreEqual("user1", ev.Usuario);
                        Assert.AreEqual("User One", ev.NombreParaMostrar);
                        Assert.AreEqual(avatarUrl, ev.AvatarUrl);

                        TestearInfo(ev.LoginInfoEncriptado);
                    })
                    .And<UsuarioSnapshot>(s =>
                    {
                        Assert.AreEqual("agrobook.usuarios-user1", s.StreamName);
                        Assert.AreEqual("User One", s.NombreParaMostrar);
                        TestearInfo(s.LoginInfoEncriptado);
                    });

            void TestearInfo(string loginInfoEncriptado)
            {
                var info = this.crypto.Deserialize<LoginInfo>(loginInfoEncriptado);
                Assert.AreEqual(0, info.Claims.Length);
                Assert.AreEqual("123", info.Password);
                Assert.AreEqual("user1", info.Usuario);
            }
        }


        [TestMethod]
        public void CuandoElUsuarioYaExisteEntoncesNoSePuedeAgregarOtroIgual()
        {
            var usuario = "agrobook.usuarios-user1";
            var avatarUrl = "picurl";
            this.sut.Given(usuario, new NuevoUsuarioCreado(TestMeta.New, "user1", "User Lastname", avatarUrl, "123"))
                    .When(s =>
                    {
                        Assert.ThrowsException<UniqueConstraintViolationException>(() =>
                        {
                            try
                            {
                                s.HandleAsync(new CrearNuevoUsuario(TestMeta.New, "user1", "User One", avatarUrl, "123", new string[0])).Wait();
                            }
                            catch (AggregateException ex)
                            {
                                throw ex.InnerException;
                            }
                        });
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
                    var result = s.HandleAsync(new IniciarSesion("non-existent-user", "fakepass", now)).Result;

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
            var loginInfo = new LoginInfo("user1", "123", new string[] { Claims.Roles.Admin });
            var eLoginInfo = this.crypto.Serialize(loginInfo);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("user1"), new NuevoUsuarioCreado(TestMeta.New, "user1", "User Name", "", eLoginInfo))
                .When(s =>
                {
                    var result = s.HandleAsync(new IniciarSesion("user1", "123", now)).Result;

                    Assert.IsTrue(result.LoginExitoso);
                    Assert.AreEqual("User Name", result.NombreParaMostrar);
                })
                .Then(e =>
                {
                    Assert.AreEqual(1, e.OfType<UsuarioInicioSesion>().Count());
                });
        }

        [TestMethod]
        public void SiElUsuarioExisteYLasCredencialesNoSonValidasEntoncesNoSePuedeIniciarSesion()
        {
            var loginInfo = new LoginInfo("user1", "123", new string[] { Claims.Roles.Admin });
            var eLoginInfo = this.crypto.Serialize(loginInfo);
            this.sut
                .Given("user1", new NuevoUsuarioCreado(TestMeta.New, "user1", "Name Lastname", "", eLoginInfo))
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

        #region Authorize
        [TestMethod]
        public void CuandoUsuarioSinNingunPermisoIntentaAccederASitioSinRestriccionesEntoncesSeLeAutoriza()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("productor", "123", new string[0]);
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given("productor", new NuevoUsuarioCreado(TestMeta.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, Claims.Roles.Tecnico);
                    Assert.IsFalse(autorizado);
                });
        }

        [TestMethod]
        public void CuandoSeQuiereAutorizarAUnUsuarioQueTienePermisosPeroNoElNecesarioEntoncesSeLeNiega()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("productor", "123", new string[] { Claims.Roles.Productor, "permiso-i" });
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given("productor", new NuevoUsuarioCreado(TestMeta.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, Claims.Roles.Tecnico);
                    Assert.IsFalse(autorizado);
                });
        }

        [TestMethod]
        public void CuandoSeQuiereAutorizarAUnUsuarioQueSiTienePermisosEntoncesSeLeConcede()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("productor", "123", new string[] { Claims.Roles.Tecnico, "permisito", Claims.Roles.Productor });
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given("agrobook.usuarios-productor", new NuevoUsuarioCreado(TestMeta.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, Claims.Roles.Tecnico);
                    Assert.IsTrue(autorizado);
                });
        }

        [TestMethod]
        public void SiElUsuarioEsAdminSiempreEsAutorizado()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("admin", "123", new string[] { Claims.Roles.Admin });
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestMeta.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, Claims.Roles.Tecnico);
                    Assert.IsTrue(autorizado);
                });
        }
        #endregion
    }
}
