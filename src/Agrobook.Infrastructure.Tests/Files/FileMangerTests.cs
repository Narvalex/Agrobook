using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Agrobook.Infrastructure.Files;

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
        public void WhenCheckingFolderIfNotExistsThenCreatesANewOne()
        {
            this.sut.EnsureFolterExists();

            Assert.IsTrue(this.sut.FolderExists);
        }
    }
}
