using Agrobook.Domain.Ap;
using Agrobook.Domain.Ap.Messages;
using Eventing.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Ap.Contratos
{
    [TestClass]
    public class DadaAdenda : ApSpec
    {
        private NuevoContrato eventoInicial;
        private NuevaAdenda eventoInicial2;

        public DadaAdenda()
        {
            this.eventoInicial = new NuevoContrato(TestFirma.New, "chorti_Nuevocontrato", "chorti", "Nuevo contrato", DateTime.Now);
            this.eventoInicial2 = new NuevaAdenda(TestFirma.New, this.eventoInicial.IdOrganizacion, this.eventoInicial.IdContrato, "chorti_Nuevocontrato_AdendaI", "Adenda I", DateTime.Now);
            this.sut.Given<Contrato>("chorti_Nuevocontrato", this.eventoInicial, this.eventoInicial2);
        }

        [Test]
        public void CuandoSeQuiereRegistrarAdendaExistenteEntoncesFalla()
        {
            var cmd = new RegistrarNuevaAdenda(TestFirma.New, this.eventoInicial.IdContrato, "Adenda I", DateTime.Now);

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
            .Then(evs =>
            {
                Assert.IsNull(evs);
            });
        }

        [Test]
        public void CuandoSeQuiereEditarAdendaNoExistenteEntoncesFalla()
        {
            var cmd = new EditarAdenda(TestFirma.New, this.eventoInicial.IdContrato, "IdAdendaNoExistente", "Adenda I Actualizada", DateTime.UtcNow);

            Assert.AreNotEqual(this.eventoInicial2.NombreDeLaAdenda, cmd.NombreDeLaAdenda);
            Assert.AreNotEqual(this.eventoInicial2.Fecha, cmd.Fecha);

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
            .Then(evs =>
            {
                Assert.IsNull(evs);
            });
        }

        [Test]
        public void SePuedeEditarNombreYFechaDeLaAdenda()
        {
            var cmd = new EditarAdenda(TestFirma.New, this.eventoInicial.IdContrato, this.eventoInicial2.IdAdenda, "Adenda I Actualizada", DateTime.Now);

            Assert.AreNotEqual(this.eventoInicial2.NombreDeLaAdenda, cmd.NombreDeLaAdenda);
            Assert.AreNotEqual(this.eventoInicial2.Fecha, cmd.Fecha);

            this.sut.When(s =>
            {
                s.HandleAsync(cmd).Wait();
            })
            .Then(evs =>
            {
                var e = evs.OfType<AdendaEditada>().Single();

                Assert.AreEqual(this.eventoInicial.IdContrato, e.IdContrato);
                Assert.AreEqual(this.eventoInicial2.IdAdenda, e.IdAdenda);
                Assert.AreEqual(cmd.NombreDeLaAdenda, e.NombreDeLaAdenda);
                Assert.AreEqual(cmd.Fecha, e.Fecha);
            });
        }
    }
}
