using Agrobook.Domain.Usuarios;
using Eventing.Core.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class OrganizacionesTests : UsuariosServiceTestBase
    {
        #region ABM Organizaciones
        [TestMethod]
        public void SiNoExisteNingunaOrganizacionEntoncesSePuedeCrearUnaCualquieraYElIdEsEnLowerCase()
        {
            this.sut.When(s =>
            {
                s.HandleAsync(new CrearNuevaOrganizacion(TestFirma.New, "Cooperativa X")).Wait();
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
                Assert.AreEqual(StreamCategoryAttribute.GetFullStreamName<Organizacion>("cooperativax"), s.StreamName);
                Assert.AreEqual("cooperativax", s.Nombre);
                Assert.AreEqual("Cooperativa X", s.NombreParaMostrar);
            });
        }

        [TestMethod]
        public void CuandoSeCreaUnaOrganizacionEntoncesSeDevuelveElNombreParaMostrarYElIdEnLowerCase()
        {
            this.sut.When(s =>
            {
                var result = s.HandleAsync(new CrearNuevaOrganizacion(TestFirma.New, "Cooperativa Equis")).Result;

                Assert.AreEqual("cooperativaequis", result.Id);
                Assert.AreEqual("Cooperativa Equis", result.Display);
            });
        }

        [TestMethod]
        public void CuandoSeCrearOrganizacionConEspaciosDeNombreAlPrincipioYAlFinalEntoncesSeHaceFullTrim()
        {
            this.sut.When(s =>
            {
                s.HandleAsync(new CrearNuevaOrganizacion(TestFirma.New, " Cooperativa X ")).Wait();
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
                Assert.AreEqual(StreamCategoryAttribute.GetFullStreamName<Organizacion>("cooperativax"), s.StreamName);
                Assert.AreEqual("cooperativax", s.Nombre);
                Assert.AreEqual("Cooperativa X", s.NombreParaMostrar);
            });
        }
        #endregion

        #region Membresia en la Organizacion
        [TestMethod]
        public void DadoUnaOrganizacionNuevaEntoncesSePuedeAgregarUsuario()
        {
            this.sut
                .Given<Organizacion>("cooperativax", new NuevaOrganizacionCreada(TestFirma.New, "cooperativax", "Cooperativa X"))
                .When(s =>
                {
                    s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestFirma.New, "cooperativax", "prod")).Wait();
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
                    Assert.IsTrue(org.TieneAlUsuarioComoMiembro("prod"));
                });
        }


        [TestMethod]
        public void DadoUnOrganizacionConUsuarioCuandoSeIntentaAgregarAlUsuarioPorSegundaVezEntoncesFalla()
        {
            this.sut
               .Given<Organizacion>("cooperativax",
                    new NuevaOrganizacionCreada(TestFirma.New, "cooperativax", "Cooperativa X"),
                    new UsuarioAgregadoALaOrganizacion(TestFirma.New, "cooperativax", "prod")
                )
               .When(s =>
               {
                   Assert.ThrowsException<InvalidOperationException>(() =>
                    {
                        try
                        {
                            s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestFirma.New, "cooperativax", "prod")).Wait();
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
                    new NuevaOrganizacionCreada(TestFirma.New, "cooperativax", "Cooperativa X")
                )
               .When(s =>
               {
                   s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestFirma.New, "cooperativax", "prod")).Wait();
                   s.HandleAsync(new AgregarUsuarioALaOrganizacion(TestFirma.New, "cooperativax", "prod2")).Wait();
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
                   Assert.IsTrue(org.TieneAlUsuarioComoMiembro("prod"));
                   Assert.IsTrue(org.TieneAlUsuarioComoMiembro("prod2"));
               });
        }


        #endregion
    }
}
