using Agrobook.Domain.Ap;
using Agrobook.Domain.Ap.Commands;
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
            var cmd = new RegistrarNuevoContrato(TestFirma.New, this.idOrg, "Nuevo contrato", DateTime.Now);
            string result = null;
            this.sut.When(s =>
            {
                result = s.HandleAsync(cmd).Result;
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
                Assert.AreEqual(e.IdContrato, result);
                Assert.AreEqual(cmd.Fecha, e.Fecha);
            });
        }

        [Test]
        public void CuandoSeQuiereRegistrarAdendaEntoncesFalla()
        {
            var cmd = new RegistrarNuevaAdenda(TestFirma.New, "chorti_Nuevocontrato", "Nueva adenda", DateTime.Now);
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
