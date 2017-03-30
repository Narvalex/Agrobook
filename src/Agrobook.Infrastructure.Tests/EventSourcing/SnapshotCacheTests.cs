using Agrobook.Core;
using Agrobook.Infrastructure.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.Caching;
using System.Threading;

namespace Agrobook.Infrastructure.Tests.EventSourcing
{
    [TestClass]
    public class SnapshotCacheTests
    {
        private ObjectCache cache;
        private SnapshotCache sut;

        public SnapshotCacheTests()
        {
            this.cache = new MemoryCache("Test");
            this.sut = new SnapshotCache(this.cache);
        }

        [TestMethod]
        public void ShouldCacheASingleSnapshot()
        {
            var snapshot = new Snapshot("test", 1);
            this.sut.Cache(snapshot);

            var cached = (ISnapshot)this.cache.Get("test");

            Assert.IsNotNull(cached);
            Assert.AreEqual("test", cached.StreamName);
            Assert.AreEqual(1, cached.Version);
        }

        [TestMethod]
        public void CanGetSingleCachedSnapshot()
        {
            var snapshot = new Snapshot("test", 2);
            this.sut.Cache(snapshot);

            Assert.IsTrue(this.sut.TryGet("test", out var cached));
            Assert.IsNotNull(cached);
            Assert.AreEqual("test", cached.StreamName);
            Assert.AreEqual(2, cached.Version);
        }

        [TestMethod]
        public void WhenCacheIsEmptyThenReturnsNull()
        {
            Assert.IsFalse(this.sut.TryGet("non-existent", out var cached));
            Assert.IsNull(cached);
        }

        [TestMethod]
        public void CanCacheAndGetMultipleSnapshots()
        {
            var s1 = new Snapshot("s1", 1);
            var s2 = new Snapshot("s2", 0);

            this.sut.Cache(s1);
            this.sut.Cache(s2);

            Assert.IsTrue(this.sut.TryGet("s1", out var c1));
            Assert.IsTrue(this.sut.TryGet("s2", out var c2));

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);

            Assert.AreEqual("s1", c1.StreamName);
            Assert.AreEqual("s2", c2.StreamName);
        }

        [TestMethod]
        public void WhenExpriresThenReturnsNull()
        {
            this.sut = new SnapshotCache(this.cache, TimeSpan.FromSeconds(2));

            var snapshot = new Snapshot("test", 2);
            this.sut.Cache(snapshot);

            Assert.IsTrue(this.sut.TryGet("test", out var cached));
            Assert.IsNotNull(cached);
            Assert.AreEqual("test", cached.StreamName);
            Assert.AreEqual(2, cached.Version);

            Thread.Sleep(2000);

            Assert.IsFalse(this.sut.TryGet("test", out var cached2));
            Assert.IsNull(cached2);
        }
    }
}
