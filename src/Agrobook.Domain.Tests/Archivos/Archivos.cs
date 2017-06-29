using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Tests.Utils;
using Agrobook.Infrastructure.Log;
using Agrobook.Infrastructure.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Domain.Tests.Archivos
{
    [TestClass]
    public class Archivos
    {
        private TestableEventSourcedService<ArchivosService> sut;

        public Archivos()
        {
            this.sut = new TestableEventSourcedService<ArchivosService>(
                r => new ArchivosService(r, new SimpleDateTimeProvider(), new ConsoleLogger(this.GetType().ToString()), new JsonTextSerializer()));
        }

        [TestMethod]
        public void TodoEmpiezaConElPrimerArchivoQueSeQuiereAgregarAUnProductor()
        {
            this.sut.When(s =>
            {

            });
        }
    }
}
