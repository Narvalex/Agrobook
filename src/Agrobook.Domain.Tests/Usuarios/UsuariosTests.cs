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
    public partial class UsuariosTests
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

                    Assert.AreEqual(UsuariosService.DefaultPassword, loginInfo.Password);
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
                .Given("user1".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "user1", "User Name", "", eLoginInfo))
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
                .Given("productor".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "productor", "Prod Apell", "", eLoginInfo))
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
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, Claims.Roles.Tecnico);
                    Assert.IsTrue(autorizado);
                });
        }
        #endregion

        #region DadaUnaActualizacionDePerfilSolicitada

        [TestMethod]
        public void CuandoElUsuarioNoExisteEntoncesError()
        {
            this.sut.When(s =>
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    try
                    {
                        s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", null, null, null, null)).Wait();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                });
            });
        }

        [TestMethod]
        public void CuandoLasOpcionesSonNulasEntoncesNoSeActualizaPerfil()
        {
            this.sut
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "admin", "admin", "avatar.png", "..."))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", null, null, null, null)).Wait();
                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 0);
                });
        }

        [TestMethod]
        public void CuandoCambioSoloElAvatarEntoncesSoloSeCambiaElAvatar()
        {
            this.sut
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "admin", "admin", "avatar.png", "..."))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", "newAvatar.png", null, null, null)).Wait();
                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 1);
                    Assert.IsTrue(events.OfType<AvatarUrlActualizado>().Single().NuevoAvatarUrl == "newAvatar.png");
                })
                .And<UsuarioSnapshot>(s =>
                {
                    Assert.IsTrue(s.AvatarUrl == "newAvatar.png");
                });
        }

        [TestMethod]
        public void CuandoSoloSeCambioElNombreParaMostrarEntoncesSoloSeCambiaElNombreParaMostrar()
        {
            this.sut
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "admin", "admin", "avatar.png", "..."))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", null, "Pe Pito", null, null)).Wait();
                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 1);
                    Assert.IsTrue(events.OfType<NombreParaMostrarActualizado>().Single().NuevoNombreParaMostrar == "Pe Pito");
                })
                .And<UsuarioSnapshot>(s =>
                {
                    Assert.IsTrue(s.NombreParaMostrar == "Pe Pito");
                });
        }

        [TestMethod]
        public void CuandoSeIntentaCambiarElPasswordConPasswordInvalidoEntoncesNoOp()
        {
            var encriptado = this.crypto.Serialize(new LoginInfo("admin", "123", null));
            this.sut
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "admin", "Ad Min", "pic1.jpg", encriptado))
                .When(s =>
                {
                    Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", null, null, "probando...", "newPass")).Wait();
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    });

                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 0);
                });
        }

        [TestMethod]
        public void CuandoSeIntentaCambiarElPasswordConPasswordValidoEntoncesSeCambia()
        {
            var encriptado = this.crypto.Serialize(new LoginInfo("admin", "123", null));
            var expectedEncriptado = this.crypto.Serialize(new LoginInfo("admin", "newPass", null));
            this.sut
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "admin", "Ad Min", "pic1.jpg", encriptado))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", null, null, "123", "newPass")).Wait();
                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 1);
                    Assert.IsTrue(events.OfType<PasswordCambiado>().Single().LoginInfoEncriptado == expectedEncriptado);
                })
                .And<UsuarioSnapshot>(s =>
                {
                    Assert.IsTrue(s.LoginInfoEncriptado == expectedEncriptado);
                });
        }

        [TestMethod]
        public void CuandoSeCambiaTodasLasOpcionesDelPerfilEntoncesSeCambianTodoALaVez()
        {
            var encriptado = this.crypto.Serialize(new LoginInfo("admin", "123", null));
            var expectedEncriptado = this.crypto.Serialize(new LoginInfo("admin", "newPass", null));
            this.sut
                .Given("admin".AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, "admin", "Ad Min", "pic1.jpg", encriptado))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestMeta.New, "admin", "pic2.jpg", "SuperAdmin", "123", "newPass")).Wait();
                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 3);
                    Assert.IsTrue(events.OfType<AvatarUrlActualizado>().Single().NuevoAvatarUrl == "pic2.jpg");
                    Assert.IsTrue(events.OfType<NombreParaMostrarActualizado>().Single().NuevoNombreParaMostrar == "SuperAdmin");
                    Assert.IsTrue(events.OfType<PasswordCambiado>().Single().LoginInfoEncriptado == expectedEncriptado);
                })
                .And<UsuarioSnapshot>(s =>
                {
                    Assert.IsTrue(s.AvatarUrl == "pic2.jpg");
                    Assert.IsTrue(s.NombreParaMostrar == "SuperAdmin");
                    Assert.IsTrue(s.LoginInfoEncriptado == expectedEncriptado);
                });
        }
        #endregion

        #region Resetear Password

        [TestMethod]
        public void DadoUnUsuarioInexistenteNoPuedeResetearPassword()
        {
            this.sut.When(s =>
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    try
                    {
                        s.HandleAsync(new ResetearPassword(TestMeta.New, "admin")).Wait();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                });
            });
        }

        [TestMethod]
        public void CuandoSeQuiereResetearPasswordEntoncesSiempreSeReseteaAlPasswordPorDefecto()
        {
            var userName = "randomUser";
            var infoEncriptado = this.crypto.Serialize(new LoginInfo(userName, "pass", null));
            this.sut
               .Given(userName.AsStreamNameOf<Usuario>(), new NuevoUsuarioCreado(TestMeta.New, userName, userName, "avatar.png", infoEncriptado))
               .When(s =>
               {
                   s.HandleAsync(new ResetearPassword(TestMeta.New, userName)).Wait();
               })
               .Then(events =>
               {
                   Assert.AreEqual(1, events.Count);

                   var e = events.OfType<PasswordReseteado>().Single();
                   Assert.AreEqual(userName, e.Usuario);

                   var info = this.crypto.Deserialize<LoginInfo>(e.LoginInfoEncriptado);
                   Assert.AreEqual(UsuariosService.DefaultPassword, info.Password);
               })
               .And<UsuarioSnapshot>(s =>
               {
                   var info = this.crypto.Deserialize<LoginInfo>(s.LoginInfoEncriptado);
                   Assert.AreEqual(UsuariosService.DefaultPassword, info.Password);
               });
        }

        #endregion
    }
}
