using Agrobook.Core;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Domain.Usuarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Agrobook.Domain.Tests.Usuarios
{
    [TestClass]
    public class OrganizacionesTests : UsuariosServiceTestBase
    {
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

        [TestMethod]
        public void DadaOrganizacionSePuedeCrearPrimerGrupo()
        {
            this.sut
            .Given<Organizacion>("coop", new NuevaOrganizacionCreada(TestMeta.New, "coop", "Coop"))
            .When(s =>
            {
                s.HandleAsync(new CrearNuevoGrupo(TestMeta.New, "coop", "Admines")).Wait();
            })
            .Then(events =>
            {
                Assert.AreEqual(1, events.Count);
            });
        }
    }
}
