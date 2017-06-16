using Agrobook.Infrastructure.Files;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Infrastructure.Tests.Files
{
    [TestClass]
    public class FileMangerTests
    {
        protected readonly FileManager sut;

        public FileMangerTests()
        {
            this.sut = new FileManager();
        }

        [TestMethod]
        [Ignore]
        public void WhenCheckingFolderIfNotExistsThenCreatesANewOne()
        {
            this.sut.EnsureFolterExists();

            Assert.IsTrue(this.sut.FolderExists);
        }
    }
}
