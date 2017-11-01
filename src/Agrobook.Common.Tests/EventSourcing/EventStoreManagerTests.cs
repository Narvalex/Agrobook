using Eventing.GetEventStore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Agrobook.Common.Tests.EventSourcing
{
    [TestClass]
    public class EventStoreManagerTests
    {
        private EventStoreManager sut = new EventStoreManager();

        [TestMethod]
        public void CanStartEventStore()
        {
            this.sut.DropAndCreateDb();
        }

        [TestMethod]
        public void CanMakeAFailFastConnection()
        {
            var connection = this.sut.GetFailFastConnection();

            Assert.IsNotNull(connection);
        }

        [TestMethod]
        public void CanMakeAResilientConnection()
        {
            Assert.IsNotNull(this.sut.ResilientConnection);
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                this.sut.TearDown(true);
            }
            catch (DirectoryNotFoundException)
            {
                // the test is just too fast!
            }
        }
    }
}
