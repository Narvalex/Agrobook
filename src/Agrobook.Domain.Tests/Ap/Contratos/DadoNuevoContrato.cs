using Agrobook.Domain.Ap;
using Agrobook.Domain.Ap.Commands;
using Eventing.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Ap.Contratos
{
    [TestClass]
    public class DadoNuevoContrato : ApSpec
    {
        private NuevoContrato eventoInicial;

        public DadoNuevoContrato()
        {
            this.eventoInicial = new NuevoContrato(TestFirma.New, "chorti_Nuevocontrato", "chorti", "Nuevo contrato", DateTime.Now);
            this.sut.Given<Contrato>("chorti_Nuevocontrato", this.eventoInicial);
        }

        [Test]
        public void SePuedeEditarNombreYFechaDelContrato()
        {
            var cmd = new EditarContrato(TestFirma.New, "chorti_Nuevocontrato", "Nuevo contrato actualizado", DateTime.UtcNow);
            this.sut.When(s =>
            {
                s.HandleAsync(cmd).Wait();
            })
            .Then(evs =>
            {
                Assert.AreEqual(1, evs.OfType<ContratoEditado>().Count());

                var e = evs.OfType<ContratoEditado>().Single();
                Assert.AreEqual(cmd.IdContrato, e.IdContrato);
                Assert.AreEqual(cmd.NombreDelContrato, e.NombreDelContrato);
                Assert.AreNotEqual(this.eventoInicial.NombreDelContrato, e.NombreDelContrato);
                Assert.AreEqual(cmd.Fecha, e.Fecha);
                Assert.AreNotEqual(this.eventoInicial.Fecha, e.Fecha);
                Assert.AreEqual(this.eventoInicial.StreamId, e.StreamId);
            });
        }

        [Test]
        [Ignore]
        public void CuandoSeQuiereRegistrarLaPrimeraAdendaEntoncesSeAcepta()
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
            })
            .And<ContratoSnapshot>(s =>
            {
                Assert.AreEqual(1, s.Adendas.Length);
                Assert.AreEqual("chorti_Nuevocontrato_AdendaI", s.Adendas[0]);
            });
        }
    }
}
