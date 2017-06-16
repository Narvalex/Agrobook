using Agrobook.Core;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class OrganizacionesTests : UsuariosServiceTestBase
    {
        #region ABM
        [TestMethod]
        public void SiNoExisteNingunaOrganizacionEntoncesSePuedeCrearUnaCualquieraYElIdEsEnLowerCase()
        {
            this.sut.When(s =>
            {
                s.HandleAsync(new CrearNuevaOrganizacion(TestMeta.New, "Cooperativa X")).Wait();
            })
            .Then(events =>
            {
                Assert.AreEqual(1, events.Count);

                var e = events.OfType<NuevaOrganizacionCreada>().Single();
                Assert.AreEqual("cooperativax", e.Identificador);
                Assert.AreEqual("Cooperativa X", e.NombreParaMostrar);
            })
            .And<OrganizacionSnapshot>(s =>
            {
                Assert.AreEqual("cooperativax".AsStreamNameOf<Organizacion>(), s.StreamName);
                Assert.AreEqual("cooperativax", s.Nombre);
                Assert.AreEqual("Cooperativa X", s.NombreParaMostrar);
            });
        }

        [TestMethod]
        public void CuandoSeCreaUnaOrganizacionEntoncesSeDevuelveElNombreParaMostrarYElIdEnLowerCase()
        {
            this.sut.When(s =>
            {
                var result = s.HandleAsync(new CrearNuevaOrganizacion(TestMeta.New, "Cooperativa Equis")).Result;

                Assert.AreEqual("cooperativaequis", result.OrgId);
                Assert.AreEqual("Cooperativa Equis", result.NombreParaMostrar);
            });
        }

        [TestMethod]
        public void CuandoSeCrearOrganizacionConEspaciosDeNombreAlPrincipioYAlFinalEntoncesSeHaceFullTrim()
        {
            this.sut.When(s =>
            {
                s.HandleAsync(new CrearNuevaOrganizacion(TestMeta.New, " Cooperativa X ")).Wait();
            })
            .Then(events =>
            {
                Assert.AreEqual(1, events.Count);

                var e = events.OfType<NuevaOrganizacionCreada>().Single();
                Assert.AreEqual("cooperativax", e.Identificador);
                Assert.AreEqual("Cooperativa X", e.NombreParaMostrar);
            })
            .And<OrganizacionSnapshot>(s =>
            {
                Assert.AreEqual("cooperativax".AsStreamNameOf<Organizacion>(), s.StreamName);
                Assert.AreEqual("cooperativax", s.Nombre);
                Assert.AreEqual("Cooperativa X", s.NombreParaMostrar);
            });
        }
        #endregion

        #region GRUPOS
        [TestMethod]
        public void DadaOrganizacionSinGruposSePuedeCrearPrimerGrupo()
        {
            this.sut
            .Given<Organizacion>("coop", new NuevaOrganizacionCreada(TestMeta.New, "coop", "Coop"))
            .When(s =>
            {
                s.HandleAsync(new CrearNuevoGrupo(TestMeta.New, "coop", "Grupo de Admines")).Wait();
            })
            .Then(events =>
            {
                Assert.AreEqual(1, events.Count);

                var e = events.OfType<NuevoGrupoCreado>().Single();
                Assert.AreEqual("grupodeadmines", e.GrupoId);
                Assert.AreEqual("Grupo de Admines", e.GrupoDisplayName);
                Assert.AreEqual("coop", e.OrganizacionId);
            })
            .And<OrganizacionSnapshot>(s =>
            {
                Assert.AreEqual("grupodeadmines", s.Grupos.Single().Key);

                IEventSourced rehidratado = new Organizacion();
                rehidratado.Rehydrate(s);

                Assert.IsTrue(((Organizacion)rehidratado).YaTieneGrupoConId("grupodeadmines"));
            });
        }

        [TestMethod]
        public void DadaOrganizacionConGrupoCuandoSeQuiereCrearNuevoGrupoIgualAExistenteEntoncesThrows()
        {
            this.sut
            .Given<Organizacion>("coop",
                new NuevaOrganizacionCreada(TestMeta.New, "coop", "Coop"),
                new NuevoGrupoCreado(TestMeta.New, "grupoadmin", "Grupo Admin", "coop"))
            .When(s =>
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    try
                    {
                        s.HandleAsync(new CrearNuevoGrupo(TestMeta.New, "coop", "Grupo admin ")).Wait();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                });
            });
        }
        #endregion

        #region Membresia en la Organizacion
        [TestMethod]
        public void DadoUnaOrganizacionNuevaEntoncesSePuedeAgregarUsuario()
        {
            this.sut
                .Given<Organizacion>("cooperativax", new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"))
                .When(s =>
                {
                    s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestMeta.New, "cooperativax", "prod")).Wait();
                })
                .Then(events =>
                {
                    var e = events.OfType<UsuarioAgregadoALaOrganizacion>().Single();

                    Assert.AreEqual("cooperativax", e.OrganizacionId);
                    Assert.AreEqual("prod", e.UsuarioId);
                })
                .And<OrganizacionSnapshot>(s =>
                {
                    Assert.AreEqual(1, s.Usuarios.Length);
                    Assert.AreEqual("prod", s.Usuarios[0]);

                    var org = s.Rehydrate<Organizacion>();
                    Assert.IsTrue(org.YaTieneAlUsuarioComoMiembro("prod"));
                });
        }

        [TestMethod]
        public void DadoUnOrganizacionConUsuarioCuandoSeIntentaAgregarAlUsuarioPorSegundaVezEntoncesFalla()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(TestMeta.New, "cooperativax", "prod")
                )
               .When(s =>
               {
                   Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestMeta.New, "cooperativax", "prod")).Wait();
                        }
                        catch (Exception ex)
                        {
                            throw ex.InnerException;
                        }
                    });
               });
        }

        [TestMethod]
        public void DadoUnOrganizacionConUsuarioCuandoSeIntentaAgregarOtroUsuarioEntoncesSucede()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(TestMeta.New, "cooperativax", "prod")
                )
               .When(s =>
               {
                   s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestMeta.New, "cooperativax", "prod2")).Wait();
               })
               .Then(events =>
               {
                   var e = events.OfType<UsuarioAgregadoALaOrganizacion>().Single();
                   Assert.AreEqual("prod2", e.UsuarioId);
               })
               .And<OrganizacionSnapshot>(s =>
               {
                   Assert.AreEqual(2, s.Usuarios.Length);

                   var org = s.Rehydrate<Organizacion>();
                   Assert.IsTrue(org.YaTieneAlUsuarioComoMiembro("prod"));
                   Assert.IsTrue(org.YaTieneAlUsuarioComoMiembro("prod2"));
               });
        }


        #endregion

        #region Agrupaciones de Usuarios dentro de la organizacion
        [TestMethod]
        public void DadaOrganizacionVaciaCuandoSeQuiereAgregarUsuarioAUnGrupoEntoncesExplota()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X")
                )
               .When(s =>
               {
                   Assert.ThrowsException<InvalidOperationException>(() =>
                   {
                       try
                       {
                           s.HandleAsync(new AgregarUsuarioAUnGrupo(TestMeta.New, "cooperativax", "braulio", "grupoInexistente")).Wait();
                       }
                       catch (Exception ex)
                       {
                           throw ex.InnerException;
                       }
                   });
               });
        }

        [TestMethod]
        public void DadaOrganizacionSinMiembrosConGrupoDefinidoCuandoSeQuiereAgregarUsuarioAlGrupoEntoncesExplota()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"),
                    new NuevoGrupoCreado(TestMeta.New, "grupito", "Grupito", "cooperativax")
                )
               .When(s =>
               {
                   Assert.ThrowsException<InvalidOperationException>(() =>
                   {
                       try
                       {
                           s.HandleAsync(new AgregarUsuarioAUnGrupo(TestMeta.New, "cooperativax", "braulio", "grupoInexistente")).Wait();
                       }
                       catch (Exception ex)
                       {
                           throw ex.InnerException;
                       }
                   });
               });
        }

        [TestMethod]
        public void DadaOrganizacionConUnUsuarioSinGruposDefinidosCuandoSeQuiereAgregarElUsuarioAUnGrupoNoExistenteEntoncesExplota()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(TestMeta.New, "cooperativax", "prod")
                )
               .When(s =>
               {
                   Assert.ThrowsException<InvalidOperationException>(() =>
                   {
                       try
                       {
                           s.HandleAsync(new AgregarUsuarioAUnGrupo(TestMeta.New, "cooperativax", "prod", "grupoInexistente")).Wait();
                       }
                       catch (Exception ex)
                       {
                           throw ex.InnerException;
                       }
                   });
               });
        }

        [TestMethod]
        public void DadaOrganizacionConUsuarioYGrupoCuandoSeQuiereAgregarElUsuarioAlGrupoEntoncesSucede()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(TestMeta.New, "cooperativax", "prod"),
                    new NuevoGrupoCreado(TestMeta.New, "grupito", "Grupito", "cooperativax")
                )
               .When(s =>
               {
                   s.HandleAsync(new AgregarUsuarioAUnGrupo(TestMeta.New, "cooperativax", "prod", "grupito")).Wait();
               })
               .Then(events =>
               {
                   Assert.AreEqual(1, events.Count);

                   var e = events.OfType<UsuarioAgregadoAUnGrupo>().Single();
                   Assert.AreEqual("grupito", e.GrupoId);
                   Assert.AreEqual("prod", e.UsuarioId);
                   Assert.AreEqual("cooperativax", e.OrganizacionId);
               })
               .And<OrganizacionSnapshot>(s =>
               {

               });
        }

        [TestMethod]
        public void DadoGrupoConUnUsuarioCuandoSeQuiereAgregarAlMismoUsuarioOtraVezEntoncesFalla()
        {
            var meta = TestMeta.New;
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(meta, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(meta, "cooperativax", "prod"),
                    new NuevoGrupoCreado(meta, "grupito", "Grupito", "cooperativax"),
                    new UsuarioAgregadoAUnGrupo(meta, "cooperativax", "prod", "grupito")
                )
               .When(s =>
               {
                   Assert.ThrowsException<InvalidOperationException>(() =>
                   {
                       try
                       {
                           s.HandleAsync(new AgregarUsuarioAUnGrupo(TestMeta.New, "cooperativax", "prod", "grupito")).Wait();
                       }
                       catch (Exception ex)
                       {
                           throw ex.InnerException;
                       }
                   });
               });
        }

        [TestMethod]
        public void DadaOrganizacionConUsuarioYGrupoCuandoSeQuiereAgregarVariosUsuariosAlGrupoEntoncesSucede()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestMeta.New, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(TestMeta.New, "cooperativax", "prod"),
                    new NuevoGrupoCreado(TestMeta.New, "grupito", "Grupito", "cooperativax"),
                    new UsuarioAgregadoAUnGrupo(TestMeta.New, "cooperativax", "prod", "grupito"),
                    new UsuarioAgregadoALaOrganizacion(TestMeta.New, "cooperativax", "prod2")
                )
               .When(s =>
               {
                   s.HandleAsync(new AgregarUsuarioAUnGrupo(TestMeta.New, "cooperativax", "prod2", "grupito")).Wait();
               })
               .Then(events =>
               {
                   Assert.AreEqual(1, events.Count);

                   var e = events.OfType<UsuarioAgregadoAUnGrupo>().Single();
                   Assert.AreEqual("grupito", e.GrupoId);
                   Assert.AreEqual("prod2", e.UsuarioId);
                   Assert.AreEqual("cooperativax", e.OrganizacionId);
               })
               .And<OrganizacionSnapshot>(s =>
               {
                   Assert.AreEqual(1, s.Grupos.Length);

                   var org = s.Rehydrate<Organizacion>();
                   Assert.IsTrue(org.YaTieneUsuarioDentroDelGrupo("grupito", "prod"));
                   Assert.IsTrue(org.YaTieneUsuarioDentroDelGrupo("grupito", "prod2"));
               });
        }

        #endregion
    }
}
