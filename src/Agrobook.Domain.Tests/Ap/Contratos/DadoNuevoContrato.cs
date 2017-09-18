using Agrobook.Domain.Ap;
using Agrobook.Domain.Ap.Messages;
using Eventing.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Ap.Contratos
{
    [TestClass]
    public class DadoNuevoContrato : ApSpec
    {
        public DadoNuevoContrato()
        {
            this.sut.Given<Contrato>("chorti_Nuevocontrato", new NuevoContrato(TestFirma.New, "chorti_Nuevocontrato", "chorti", "Nuevo contrato", DateTime.Now));
        }

        [Test]
        public void CuandoSeQuiereRegistrarAdendaEntoncesSeAcepta()
        {
            var cmd = new RegistrarNuevaAdenda(TestFirma.New, "chorti_Nuevocontrato", "Adenda I", DateTime.Now);
            string result = null;
            this.sut.When(s =>
            {
                result = s.HandleAsync(cmd).Result;
            })
            .Then(evs =>
            {
                Assert.AreEqual(1, evs.OfType<NuevaAdenda>().Count());

                var e = evs.OfType<NuevaAdenda>().Single();
                Assert.AreEqual("chorti_Nuevocontrato_AdendaI", e.IdAdenda);
                Assert.AreEqual("chorti_Nuevocontrato", e.IdContrato);
                Assert.AreEqual("chorti", e.IdOrganizacion);
                Assert.AreEqual("Adenda I", e.NombreDeLaAdenda);
                Assert.AreEqual(e.IdAdenda, result);
                Assert.AreEqual(e.IdContrato, e.StreamId);
            });
        }
    }
}
