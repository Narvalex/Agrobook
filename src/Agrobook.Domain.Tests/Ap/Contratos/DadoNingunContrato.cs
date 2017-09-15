using Agrobook.Domain.Ap.Messages;
using Eventing.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Ap.Contratos
{
    [TestClass]
    public class DadoNingunContrato : ApSpec
    {
        private readonly string idOrg = "chorti";

        [Test]
        public void CuandoSeQuiereRegistrarContratoSeAcepta()
        {
            var cmd = new RegistrarNuevoContrato(this.idOrg, "Nuevo contrato");
            this.sut.When(s =>
            {
                s.HandleAsync(cmd).Wait();
            })
            .Then(events =>
            {
                Assert.AreEqual(1, events.Count);
                Assert.AreEqual(1, events.OfType<NuevoContrato>().Count());

                var e = events.OfType<NuevoContrato>().Single();
                Assert.AreEqual("chorti_Nuevocontrato", e.IdContrato);
                Assert.AreEqual("chorti", e.IdOrganizacion);
                Assert.AreEqual("Nuevo contrato", e.NombreDelContrato);
                Assert.AreEqual(e.IdContrato, e.StreamId);
            });
        }

        [Test]
        public void CuandoSeQuiereRegistrarAdendaEntoncesFalla()
        {
            var cmd = new RegistrarNuevaAdenda("chorti_Nuevocontrato", "Nueva adenda");
            this.sut.When(s =>
            {
                Assert.ThrowsException<InvalidOperationException>(() =>
                {
                    try
                    {
                        s.HandleAsync(cmd).Wait();
                    }
                    catch (Exception ex)
                    {
                        throw ex.InnerException;
                    }
                });
            })
            .Then(events =>
            {
                Assert.IsNull(events);
            });
        }
    }
}
