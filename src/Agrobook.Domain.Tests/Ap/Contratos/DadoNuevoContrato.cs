using Agrobook.Domain.Ap;
using Agrobook.Domain.Ap.Messages;
using Eventing.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Agrobook.Domain.Tests.Ap.Contratos
{
    [TestClass]
    public class DadoNuevoContrato : ApSpec
    {
        public DadoNuevoContrato()
        {
            this.sut.Given<Contrato>("chorti_Nuevocontrato", new NuevoContrato("chorti_Nuevocontrato", "chorti", "Nuevo contrato"));
        }

        [Test]
        public void CuandoSeQuiereRegistrarAdendaEntoncesSeAcepta()
        {
            var cmd = new RegistrarNuevaAdenda("chorti_Nuevocontrato", "Adenda I");
            this.sut.When(s =>
            {
                s.HandleAsync(cmd).Wait();
            })
            .Then(evs =>
            {
                Assert.AreEqual(1, evs.OfType<NuevaAdenda>().Count());

                var e = evs.OfType<NuevaAdenda>().Single();
                Assert.AreEqual("chorti_Nuevocontrato_AdendaI", e.IdAdenda);
                Assert.AreEqual("chorti_Nuevocontrato", e.IdContrato);
                Assert.AreEqual("chorti", e.IdOrganizacion);
                Assert.AreEqual("Adenda I", e.NombreDeLaAdenda);
                Assert.AreEqual(e.IdContrato, e.StreamId);
            });
        }
    }
}
