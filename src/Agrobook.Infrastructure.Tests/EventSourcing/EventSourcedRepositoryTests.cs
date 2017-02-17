using Agrobook.Infrastructure.EventSourcing;
using Agrobook.Infrastructure.Serialization;
using Agrobook.Infrastructure.Tests.EventSourcing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace Agrobook.Infrastructure.Tests.EventSourcing
{
    [TestClass]
    public class EventSourcedRepositoryTests
    {
        private readonly EventStoreManager esManager;
        private readonly EventSourcedRepository sut;

        public EventSourcedRepositoryTests()
        {
            this.esManager = new EventStoreManager();
            this.esManager.InitializeDb();
            Thread.Sleep(TimeSpan.FromSeconds(3)); // Warmingup the Db
            this.sut = new EventSourcedRepository(this.esManager.GetFailFastConnection, new JsonSerializer());
        }

        [TestMethod]
        public void GivenNoStreamWhenTryingToGetThenReturnsNull()
        {
            var streamName = "simple-aggregate-856987";
            var aggregate = this.sut.GetAsync<AggregateWithSimpleSnapshot>(streamName).Result;

            Assert.IsNull(aggregate);
        }

        [TestCleanup]
        public void TearDown()
        {
            try
            {
                this.esManager.TearDown(true);
            }
            catch (DirectoryNotFoundException)
            {
                // the test is just too fast!
            }
        }
    }
}
