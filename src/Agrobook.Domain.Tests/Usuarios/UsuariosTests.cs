using Agrobook.Domain.Usuarios;
using Agrobook.Domain.Usuarios.Login;
using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static Agrobook.Domain.Usuarios.Login.ClaimDef;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class UsuariosTests : UsuariosServiceTestBase
    {
        [TestMethod]
        public void ElNombreDelStreamSeGeneraCorrectamente()
        {
            var usuario = new Usuario();
            usuario.Emit(new NuevoUsuarioCreado(TestFirma.New, "user", "Us Er", "avatar.png", "encriptedata"));

            Assert.AreEqual("agrobook.usuarios-user", usuario.StreamName);
        }

        #region ABM Usuarios
        //[TestMethod]
        //public void SiPuedeDetectarQueNoExisteTodaviaElUsuarioAdmin()
        //{
        //    this.sut
        //        .Given()
        //        .When(s =>
        //        {
        //            Assert.IsFalse(s.ExisteUsuarioAdmin);
        //        });
        //}

        [TestMethod]
        public void SePuedeCrearElUsuarioAdminSiNoExiste()
        {
            this.sut
                .When(s =>
                {
                    s.CrearUsuarioAdminAsync().Wait();
                })
                .Then(e =>
                {
                    Assert.AreEqual(1, e.Count);
                    Assert.AreEqual("admin", e.OfType<NuevoUsuarioCreado>().Single().Usuario);

                    var loginInfo = this.crypto.Deserialize<LoginInfo>(e.OfType<NuevoUsuarioCreado>().Single().LoginInfoEncriptado);

                    Assert.AreEqual(UsuariosConstants.DefaultPassword, loginInfo.Password);
                });
        }

        //[TestMethod]
        //public void SePuedeDetectarQueSiExisteElUsuarioAdminUnaVezCreado()
        //{
        //    this.sut
        //        .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestMeta.New, "admin", "admin", "*.png", "password"))
        //        .When(s =>
        //        {
        //            Assert.IsTrue(s.ExisteUsuarioAdmin);
        //        });
        //}

        [TestMethod]
        public void SePuedeCrearUsuarioNuevoSinEspecificarNingunPermisoPeroElMismoQuedaraComoInvitado()
        {
            var avatarUrl = "app/avatar.png";
            this.sut.When(s =>
                        s.HandleAsync(new CrearNuevoUsuario(TestFirma.New, "user1", "User One", avatarUrl, "123", null)).Wait())
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
                Assert.AreEqual(1, info.Claims.Length);
                Assert.AreEqual(ClaimDef.Roles.Invitado, info.Claims[0]);
                Assert.AreEqual("123", info.Password);
                Assert.AreEqual("user1", info.Usuario);
            }
        }

        [TestMethod]
        public void NoSePuedeCrearUsuarioConEspaciosEntreCaracteres()
        {
            var avatarUrl = "app/avatar.png";
            this.sut.When(s =>
                    {
                        Assert.ThrowsException<ArgumentException>(() =>
                        {
                            try
                            {
                                s.HandleAsync(new CrearNuevoUsuario(
                                TestFirma.New,
                                "user with spaces ", "User One", avatarUrl, "123", null)).Wait();
                            }
                            catch (Exception ex)
                            {
                                throw ex.InnerException;
                            }
                        });
                    });
        }

        [TestMethod]
        [Ignore] // is ignored because the in-memory event store does not check set validation
        public void CuandoElUsuarioYaExisteEntoncesNoSePuedeAgregarOtroIgual()
        {
            var usuario = "agrobook.usuarios-user1";
            var avatarUrl = "picurl";
            this.sut.Given(usuario, new NuevoUsuarioCreado(TestFirma.New, "user1", "User Lastname", avatarUrl, "123"))
                    .When(s =>
                    {
                        Assert.ThrowsException<UniqueConstraintViolationException>(() =>
                        {
                            try
                            {
                                s.HandleAsync(new CrearNuevoUsuario(TestFirma.New, "user1", "User One", avatarUrl, "123", new string[0])).Wait();
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
                .When(s =>
                {
                    var result = s.HandleAsync(new IniciarSesion("non-existent-user", "fakepass", now)).Result;

                    Assert.IsFalse(result.LoginExitoso);
                })
                .Then(e =>
                {
                    Assert.IsNull(e);
                });
        }

        [TestMethod]
        public void SiElUsuarioExisteYLasCredencialesSonValidasSePuedeIniciarSesion()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("user1", "123", new string[] { ClaimDef.Roles.Admin });
            var eLoginInfo = this.crypto.Serialize(loginInfo);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("user1"), new NuevoUsuarioCreado(TestFirma.New, "user1", "User Name", "", eLoginInfo))
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
            var loginInfo = new LoginInfo("user1", "123", new string[] { ClaimDef.Roles.Admin });
            var eLoginInfo = this.crypto.Serialize(loginInfo);
            this.sut
                .Given("user1", new NuevoUsuarioCreado(TestFirma.New, "user1", "Name Lastname", "", eLoginInfo))
                .When(s =>
                {
                    var result = s.HandleAsync(new IniciarSesion("user1", "wrongPassword", DateTime.Now)).Result;

                    Assert.IsFalse(result.LoginExitoso);
                })
                .Then(e =>
                {
                    Assert.IsNull(e);
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
                .Given("productor", new NuevoUsuarioCreado(TestFirma.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, ClaimDef.Roles.Tecnico);
                    Assert.IsFalse(autorizado);
                });
        }

        [TestMethod]
        public void CuandoSeQuiereAutorizarAUnUsuarioQueTienePermisosPeroNoElNecesarioEntoncesSeLeNiega()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("productor", "123", new string[] { ClaimDef.Roles.Productor, "permiso-i" });
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given("productor", new NuevoUsuarioCreado(TestFirma.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, ClaimDef.Roles.Tecnico);
                    Assert.IsFalse(autorizado);
                });
        }

        [TestMethod]
        public void CuandoSeQuiereAutorizarAUnUsuarioQueSiTienePermisosEntoncesSeLeConcede()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("productor", "123", new string[] { ClaimDef.Roles.Tecnico, "permisito", ClaimDef.Roles.Productor });
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("productor"), new NuevoUsuarioCreado(TestFirma.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, ClaimDef.Roles.Tecnico);
                    Assert.IsTrue(autorizado);
                });
        }

        [TestMethod]
        public void SiElUsuarioEsAdminSiempreEsAutorizado()
        {
            var now = DateTime.Now;
            var loginInfo = new LoginInfo("admin", "123", new string[] { ClaimDef.Roles.Admin });
            var eLoginInfo = this.crypto.Serialize(loginInfo);

            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestFirma.New, "productor", "Prod Apell", "", eLoginInfo))
                .When(s =>
                {
                    var autorizado = s.TryAuthorize(eLoginInfo, ClaimDef.Roles.Tecnico);
                    Assert.IsTrue(autorizado);
                });
        }
        #endregion

        #region Actualizacion de Perfil

        [TestMethod]
        public void CuandoElUsuarioNoExisteEntoncesError()
        {
            this.sut.When(s =>
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    try
                    {
                        s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", null, null, null, null)).Wait();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                });
            });
        }

        [TestMethod]
        [Ignore]
        public void CuandoLasOpcionesSonNulasEntoncesNoSeActualizaPerfil()
        {
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestFirma.New, "admin", "admin", "avatar.png", "..."))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", null, null, null, null)).Wait();
                })
                .Then(events =>
                {
                    Assert.IsTrue(events.Count == 0);
                });
        }

        [TestMethod]
        [Ignore]
        public void CuandoCambioSoloElAvatarEntoncesSoloSeCambiaElAvatar()
        {
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestFirma.New, "admin", "admin", "avatar.png", "..."))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", "newAvatar.png", null, null, null)).Wait();
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
        [Ignore]
        public void CuandoSoloSeCambioElNombreParaMostrarEntoncesSoloSeCambiaElNombreParaMostrar()
        {
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestFirma.New, "admin", "admin", "avatar.png", "..."))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", null, "Pe Pito", null, null)).Wait();
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
        [Ignore]
        public void CuandoSeIntentaCambiarElPasswordConPasswordInvalidoEntoncesNoOp()
        {
            var encriptado = this.crypto.Serialize(new LoginInfo("admin", "123", null));
            this.sut
                .Given<Usuario>("admin", new NuevoUsuarioCreado(TestFirma.New, "admin", "Ad Min", "pic1.jpg", encriptado))
                .When(s =>
                {
                    Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", null, null, "probando...", "newPass")).Wait();
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    });

                });
        }

        [TestMethod]
        [Ignore]
        public void CuandoSeIntentaCambiarElPasswordConPasswordValidoEntoncesSeCambia()
        {
            var encriptado = this.crypto.Serialize(new LoginInfo("admin", "123", null));
            var expectedEncriptado = this.crypto.Serialize(new LoginInfo("admin", "newPass", null));
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestFirma.New, "admin", "Ad Min", "pic1.jpg", encriptado))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", null, null, "123", "newPass")).Wait();
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
        [Ignore]
        public void CuandoSeCambiaTodasLasOpcionesDelPerfilEntoncesSeCambianTodoALaVez()
        {
            var encriptado = this.crypto.Serialize(new LoginInfo("admin", "123", null));
            var expectedEncriptado = this.crypto.Serialize(new LoginInfo("admin", "newPass", null));
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), new NuevoUsuarioCreado(TestFirma.New, "admin", "Ad Min", "pic1.jpg", encriptado))
                .When(s =>
                {
                    s.HandleAsync(new ActualizarPerfil(TestFirma.New, "admin", "pic2.jpg", "SuperAdmin", "123", "newPass")).Wait();
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
        [Ignore]
        public void DadoUnUsuarioInexistenteNoPuedeResetearPassword()
        {
            this.sut.When(s =>
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    try
                    {
                        s.HandleAsync(new ResetearPassword(TestFirma.New, "admin")).Wait();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                });
            });
        }

        [TestMethod]
        [Ignore]
        public void CuandoSeQuiereResetearPasswordEntoncesSiempreSeReseteaAlPasswordPorDefecto()
        {
            var userName = "randomUser";
            var infoEncriptado = this.crypto.Serialize(new LoginInfo(userName, "pass", null));
            this.sut
               .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(userName), new NuevoUsuarioCreado(TestFirma.New, userName, userName, "avatar.png", infoEncriptado))
               .When(s =>
               {
                   s.HandleAsync(new ResetearPassword(TestFirma.New, userName)).Wait();
               })
               .Then(events =>
               {
                   Assert.AreEqual(1, events.Count);

                   var e = events.OfType<PasswordReseteado>().Single();
                   Assert.AreEqual(userName, e.Usuario);

                   var info = this.crypto.Deserialize<LoginInfo>(e.LoginInfoEncriptado);
                   Assert.AreEqual(UsuariosConstants.DefaultPassword, info.Password);
               })
               .And<UsuarioSnapshot>(s =>
               {
                   var info = this.crypto.Deserialize<LoginInfo>(s.LoginInfoEncriptado);
                   Assert.AreEqual(UsuariosConstants.DefaultPassword, info.Password);
               });
        }

        #endregion

        #region Creacion de Usuarios

        [TestMethod]
        public void DadoUsuarioQueQuiereCrearOtroUsuarioCuandoPasaTokenNuloEntoncesRetornaNulo()
        {
            var userName = "admin";
            var infoEncriptado = this.crypto.Serialize(new LoginInfo(userName, "pass", null));
            this.sut.Given<Usuario>("user", new NuevoUsuarioCreado(TestFirma.New, userName, "Admin", "pic.png", infoEncriptado))
                .When(s =>
                {
                    var claims = s.ObtenerListaDeClaimsDisponiblesParaElUsuario(null).Result;

                    Assert.IsNull(claims);
                });
        }

        [TestMethod]
        public void CuandoAdminQuiereCrearUsuarioSeLeHabilitanTodosLosRolesYPermisos()
        {
            var userName = "admin";
            var infoEncriptado = this.crypto.Serialize(new LoginInfo(userName, "pass", new string[] { Roles.Admin }));
            this.sut.Given<Usuario>(userName, new NuevoUsuarioCreado(TestFirma.New, userName, "Admin", "pic.png", infoEncriptado))
                .When(s =>
                {
                    var claims = s.ObtenerListaDeClaimsDisponiblesParaElUsuario(infoEncriptado).Result;

                    Assert.IsNotNull(claims);
                    Assert.AreEqual(ClaimProvider.ClaimCount, claims.Length);

                    Assert.IsTrue(ClaimProvider
                        .Todos.Values
                        .All(c => claims.Any(x => x.Id == c.Id))
                    );
                });
        }

        [TestMethod]
        public void CuandoGerenteQuiereCrearUsuariosEntoncesPuedeCrearTodosMenosAdminYGerente()
        {
            var userName = "juancito";
            var infoEncriptado = this.crypto.Serialize(new LoginInfo(userName, "pass", new string[] { Roles.Gerente }));
            this.sut.Given<Usuario>(userName, new NuevoUsuarioCreado(TestFirma.New, userName, userName, "pic.png", infoEncriptado))
                .When(s =>
                {
                    var claims = s.ObtenerListaDeClaimsDisponiblesParaElUsuario(infoEncriptado).Result;

                    Assert.IsNotNull(claims);
                    Assert.AreEqual(ClaimProvider.ClaimCount - 2, claims.Length);

                    Assert.IsTrue(ClaimProvider
                        .Todos.Values
                        .Where(c => c.Id != Roles.Admin && c.Id != Roles.Gerente)
                        .All(c => claims.Any(x => x.Id == c.Id))
                    );
                });
        }

        [TestMethod]
        public void CuandoTecnicoQuiereCrearUsuarioEntoncesSolamentePuedeCrearProductores()
        {
            var userName = "juancito";
            var infoEncriptado = this.crypto.Serialize(
                new LoginInfo(userName, "pass", new string[] { Roles.Tecnico }));
            this.sut.Given<Usuario>(userName, new NuevoUsuarioCreado(TestFirma.New, userName, userName, "pic.png", infoEncriptado))
                .When(s =>
                {
                    var claims = s.ObtenerListaDeClaimsDisponiblesParaElUsuario(infoEncriptado).Result;

                    Assert.IsNotNull(claims);
                    Assert.AreEqual(1, claims.Length);
                    Assert.AreEqual(Roles.Productor, claims.Single().Id);
                });
        }

        [TestMethod]
        public void CuandoProductorQuiereCrearUsuarioEntoncesNoSeLeDaNingunaPosibilidad()
        {
            var userName = "juancito";
            var infoEncriptado = this.crypto.Serialize(
                new LoginInfo(userName, "pass", new string[] { Roles.Productor }));
            this.sut.Given<Usuario>(userName, new NuevoUsuarioCreado(TestFirma.New, userName, userName, "pic.png", infoEncriptado))
                .When(s =>
                {
                    var claims = s.ObtenerListaDeClaimsDisponiblesParaElUsuario(infoEncriptado).Result;

                    Assert.IsNull(claims);
                });
        }

        [TestMethod]
        public void CuandoInvitadoQuiereCrearUsuarioEntoncesNoSeLeDaNingunaPosibilidad()
        {
            var userName = "juancito";
            var infoEncriptado = this.crypto.Serialize(
                new LoginInfo(userName, "pass", new string[] { Roles.Invitado }));
            this.sut.Given<Usuario>(userName, new NuevoUsuarioCreado(TestFirma.New, userName, userName, "pic.png", infoEncriptado))
                .When(s =>
                {
                    var claims = s.ObtenerListaDeClaimsDisponiblesParaElUsuario(infoEncriptado).Result;

                    Assert.IsNull(claims);
                });
        }

        #endregion

        #region Edicion de Roles y Permisos

        // REMOVER ROLES Y PERMISOS

        [TestMethod]
        public void DadoUsuarioAdminNoSePuedeRemoverPermisoDeAdminPorQueEsElAdmin()
        {
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo("admin", "pass", new string[] { ClaimDef.Roles.Admin }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, "admin", "admin", "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>("admin"), eventoUsuarioCreado)
                .When(s =>
                {
                    Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new RetirarPermiso(TestFirma.New, "admin", ClaimDef.Roles.Admin)).Wait();
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    });
                });
        }

        [TestMethod]
        [Ignore]
        public void DadoUsuarioConUnicoRolDeProductorSeLePuedeRetirarElRolYQuedaraComoInvitado()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Productor }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    s.HandleAsync(new RetirarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Productor))
                    .Wait();
                })
                .Then(events =>
                {
                    Assert.AreEqual(2, events.Count);

                    var retirado = events.OfType<PermisoRetiradoDelUsuario>().Single();
                    var otorgado = events.OfType<PermisoOtorgadoAlUsuario>().Single();

                    Assert.AreEqual(ClaimDef.Roles.Productor, retirado.Permiso);
                    Assert.AreEqual(ClaimDef.Roles.Invitado, otorgado.Permiso);
                });
        }

        [TestMethod]
        [Ignore]
        public void DadoUsuarioConUnicoRolDeTecnicoSeLePuedeRetirarElRolYQuedaraComoInvitado()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Tecnico }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    s.HandleAsync(new RetirarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Tecnico))
                    .Wait();
                })
                .Then(events =>
                {
                    Assert.AreEqual(2, events.Count);

                    var retirado = events.OfType<PermisoRetiradoDelUsuario>().Single();
                    var otorgado = events.OfType<PermisoOtorgadoAlUsuario>().Single();

                    Assert.AreEqual(ClaimDef.Roles.Tecnico, retirado.Permiso);
                    Assert.AreEqual(ClaimDef.Roles.Invitado, otorgado.Permiso);
                });
        }

        [TestMethod]
        [Ignore]
        public void DadoUsuarioConUnicoRolDeGerenteSeLePuedeRetirarElRolYQuedaraComoInvitado()
        {

            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Gerente }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    s.HandleAsync(new RetirarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Gerente))
                    .Wait();
                })
                .Then(events =>
                {
                    Assert.AreEqual(2, events.Count);

                    var retirado = events.OfType<PermisoRetiradoDelUsuario>().Single();
                    var otorgado = events.OfType<PermisoOtorgadoAlUsuario>().Single();

                    Assert.AreEqual(ClaimDef.Roles.Gerente, retirado.Permiso);
                    Assert.AreEqual(ClaimDef.Roles.Invitado, otorgado.Permiso);
                });
        }

        [TestMethod]
        [Ignore]
        public void DadoUsuarioConUnicoRolDeAdminSeLePuedeRetirarElRolYQuedaraComoInvitado()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Admin }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    s.HandleAsync(new RetirarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Admin))
                    .Wait();
                })
                .Then(events =>
                {
                    Assert.AreEqual(2, events.Count);

                    var retirado = events.OfType<PermisoRetiradoDelUsuario>().Single();
                    var otorgado = events.OfType<PermisoOtorgadoAlUsuario>().Single();

                    Assert.AreEqual(ClaimDef.Roles.Admin, retirado.Permiso);
                    Assert.AreEqual(ClaimDef.Roles.Invitado, otorgado.Permiso);
                });
        }

        [TestMethod]
        [Ignore]
        public void DadoUsuarioConRolDeGerenteYAdminSiSeLeQuitaAdminEntoncesQuedaraComoGerente()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Gerente, ClaimDef.Roles.Admin }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    s.HandleAsync(new RetirarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Admin))
                    .Wait();
                })
                .Then(events =>
                {
                    Assert.AreEqual(1, events.Count);

                    var retirado = events.OfType<PermisoRetiradoDelUsuario>().Single();

                    Assert.AreEqual(ClaimDef.Roles.Admin, retirado.Permiso);
                });
        }

        [TestMethod]
        [Ignore]
        public void DadoUsuarioQueSoloEsInvitadoNoSeLePuedeRetirarEseRolPorQueEsElUnicoQueTiene()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Invitado }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new RetirarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Invitado))
                            .Wait();
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    });
                });
        }

        // OTORGAR ROLES Y PERMISOS

        [TestMethod]
        [Ignore]
        public void SiElUsuarioYaTieneUnRolOPermisoQueSeQuiereOtorgarEntoncesThrows()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Invitado }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new OtorgarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Invitado))
                            .Wait();
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    });
                });
        }

        [TestMethod]
        [Ignore]
        public void SiElUsuarioNoTieneUnPermisoYSeLeQuiereOtorgarEntoncesFunciona()
        {
            var nombreDeUsuario = "jorgito";
            var loginInfoEncriptado = this.crypto.Serialize(new LoginInfo(nombreDeUsuario, "pass", new string[] { ClaimDef.Roles.Invitado }));
            var eventoUsuarioCreado = new NuevoUsuarioCreado(TestFirma.New, nombreDeUsuario, nombreDeUsuario, "avatar.png", loginInfoEncriptado);
            this.sut
                .Given(StreamCategoryAttribute.GetFullStreamName<Usuario>(nombreDeUsuario), eventoUsuarioCreado)
                .When(s =>
                {
                    s.HandleAsync(new OtorgarPermiso(TestFirma.New, nombreDeUsuario, ClaimDef.Roles.Admin))
                    .Wait();
                })
                .Then(events =>
                {
                    Assert.AreEqual(1, events.Count);

                    var e = events.OfType<PermisoOtorgadoAlUsuario>().Single();

                    Assert.AreEqual(ClaimDef.Roles.Admin, e.Permiso);
                });
        }

        #endregion 
    }
}
