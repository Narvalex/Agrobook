using Agrobook.Domain.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Agrobook.Domain.Tests
{
    [TestClass]
    public class MesajesAuditablesTests
    {
        [TestMethod]
        public void LosMetadatosSePuedenCargarSoloSiNoSeCargoAntes()
        {
            var now = DateTime.Now;
            var mensaje = new MensajeAuditable(null);
            Assert.IsNull(mensaje.Firma);

            var metadatos = new Firma("test", now);
            mensaje.TrySet(metadatos);

            Assert.AreEqual("test", mensaje.Firma.Usuario);
            Assert.AreEqual(now, mensaje.Firma.Timestamp);
        }
    }
}
