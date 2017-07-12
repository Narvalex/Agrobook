using Agrobook.Domain.Archivos;
using Agrobook.Domain.Archivos.Services;
using Agrobook.Domain.Tests.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Agrobook.Domain.Tests.Archivos
{
    [TestClass]
    public class ArchivosTests
    {
        private TestableFileManager fileManager;
        private TestableEventSourcedService<ArchivosService> sut;

        public ArchivosTests()
        {
            this.fileManager = new TestableFileManager();

            this.sut = new TestableEventSourcedService<ArchivosService>(
                r => new ArchivosService(this.fileManager, r, new SimpleDateTimeProvider()));
        }

        #region Uploads
        [TestMethod]
        public void TodoEmpiezaConElPrimerArchivoQueSeQuiereAgregarAUnProductor()
        {
            var archivo = new ArchivoDescriptor("foto", "jpg", DateTime.Now, null, 1024);
            this.sut.When(s =>
            {
                var result = s.HandleAsync(new AgregarArchivoAColeccion(TestMeta.New, "fulano", archivo, null)).Result;

                Assert.IsTrue(result.Exitoso);
                Assert.IsFalse(result.YaExiste);
            })
            .Then(events =>
            {
                Assert.AreEqual(2, events.Count);
                Assert.AreEqual(1, events.OfType<NuevaColeccionDeArchivosDelProductorCreada>().Count());
                Assert.AreEqual(1, events.OfType<NuevoArchivoAgregadoALaColeccion>().Count());
            })
            .And<ColeccionDeArchivosDelProductorSnapshot>(s =>
            {
                var coleccion = s.Rehydrate<ColeccionDeArchivosDelProductor>();

                Assert.IsTrue(coleccion.YaTieneArchivo("foto"));
            });
        }

        [TestMethod]
        public void SiElArchivoYaExisteEnLaColeccionEntonesElResultadoEsQueYaExiste()
        {
            var archivo = new ArchivoDescriptor("foto", "jpg", DateTime.Now, null, 1024);
            this.sut.When(s =>
            {
                var command = new AgregarArchivoAColeccion(TestMeta.New, "fulano", archivo, null);
                s.HandleAsync(command).Wait();
                var result = s.HandleAsync(command).Result;

                Assert.IsFalse(result.Exitoso);
                Assert.IsTrue(result.YaExiste);
            })
            .Then(events =>
            {
                // Solamente de la primera operacion es esto...
                Assert.AreEqual(2, events.Count);
                Assert.AreEqual(1, events.OfType<NuevaColeccionDeArchivosDelProductorCreada>().Count());
                Assert.AreEqual(1, events.OfType<NuevoArchivoAgregadoALaColeccion>().Count());
            })
            .And<ColeccionDeArchivosDelProductorSnapshot>(s =>
            {
                // Tambien esto es de la primera operacion
                var coleccion = s.Rehydrate<ColeccionDeArchivosDelProductor>();

                Assert.IsTrue(coleccion.YaTieneArchivo("foto"));
            });
        }

        [TestMethod]
        public void SiElArchivoNoExisteEnColecionPeroSiEnDiscoEntonesElResultadoEsQueYaExiste()
        {
            this.fileManager.RetornarResultadoExitoso = false;
            var archivo = new ArchivoDescriptor("foto", "jpg", DateTime.Now, null, 1024);
            this.sut.When(s =>
            {
                var command = new AgregarArchivoAColeccion(TestMeta.New, "fulano", archivo, null);
                var result = s.HandleAsync(command).Result;

                Assert.IsFalse(result.Exitoso);
                Assert.IsTrue(result.YaExiste);
            })
            .Then(events =>
            {
                Assert.AreEqual(0, events.Count);
            })
            .And<ColeccionDeArchivosDelProductorSnapshot>(s =>
            {
                Assert.IsNull(s);
            });
        }

        [TestMethod]
        public void SolamenteSePuedenGuardarArchivosConDistintosNombres()
        {
            var archivo = new ArchivoDescriptor("foto", "jpg", DateTime.Now, null, 1024);
            this.sut.When(s =>
            {
                var command = new AgregarArchivoAColeccion(TestMeta.New, "fulano", archivo, null);
                s.HandleAsync(command).Wait();
                command = new AgregarArchivoAColeccion(TestMeta.New, "fulano", new ArchivoDescriptor("foto2", "jpg", DateTime.Now, null, 1024), null);
                var result = s.HandleAsync(command).Result;

                Assert.IsTrue(result.Exitoso);
                Assert.IsFalse(result.YaExiste);
            })
            .Then(events =>
            {
                Assert.AreEqual(3, events.Count);
                Assert.AreEqual(1, events.OfType<NuevaColeccionDeArchivosDelProductorCreada>().Count());
                Assert.AreEqual(2, events.OfType<NuevoArchivoAgregadoALaColeccion>().Count());
            })
            .And<ColeccionDeArchivosDelProductorSnapshot>(s =>
            {
                var coleccion = s.Rehydrate<ColeccionDeArchivosDelProductor>();

                Assert.IsTrue(coleccion.YaTieneArchivo("foto"));
                Assert.IsTrue(coleccion.YaTieneArchivo("foto2"));
            });
        }
        #endregion

        #region Downloads
        [TestMethod]
        public void CuandoUnArchivoDeDescargoEntoncesSeRegistraEnElServidor()
        {
            var archivo = new ArchivoDescriptor("foto", "jpg", DateTime.Now, null, 1024);
            this.sut
            .When(s =>
            {
                s.HandleAsync(new AgregarArchivoAColeccion(TestMeta.New, "fulano", archivo, null)).Wait();
                s.HandleAsync(new RegistrarDescargaExitosa(TestMeta.New, "fulano", "foto")).Wait();
            })
            .Then(events =>
            {
                Assert.AreEqual(1, events.OfType<ArchivoDescargadoExitosamente>().Count());

                var e = events.OfType<ArchivoDescargadoExitosamente>().Single();

                Assert.AreEqual("fulano", e.Productor);
                Assert.AreEqual(archivo.Nombre, e.NombreArchivo);
                Assert.AreEqual(archivo.Size, e.Size);
            });
        }
        #endregion
    }
}
