using Agrobook.Infrastructure.EventSourcing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Infrastructure.Tests.EventSourcing
{
    [TestClass]
    public class EventStoreManagerTests
    {
        [TestMethod]
        public void CanStartEventStore()
        {
            EventStoreManager.InitializeDb();
        }

        [TestCleanup]
        public void TearDown()
        {
            EventStoreManager.TearDown();
        }
    }
}
