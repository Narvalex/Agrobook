﻿using Agrobook.Infrastructure.EventSourcing;
using Agrobook.Infrastructure.Serialization;
using Agrobook.Infrastructure.Tests.EventSourcing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading;

namespace Agrobook.Infrastructure.Tests.EventSourcing
{
    [TestClass]
    public class EventSourcedRepositoryIntegrationTests
    {

        private EventStoreManager esManager;
        private EventSourcedRepository sut;
        private TestableRealTimeSnapshotter snapshotter;

        [TestInitialize]
        public void Init()
        {
            this.esManager = new EventStoreManager();
            this.TearDown(); // Cleans trash if remained...
            this.esManager.InitializeDb();
            Thread.Sleep(TimeSpan.FromSeconds(4)); // Warmingup the Db

            this.snapshotter = new TestableRealTimeSnapshotter();
            this.sut = new EventSourcedRepository(this.esManager.GetFailFastConnection, new JsonTextSerializer(), this.snapshotter);
        }

        [TestMethod]
        public void EventSourcedRepositoryTestSuite()
        {
            // Single event tests
            this.t1_GivenNoStreamWhenTryingToGetThenReturnsNull();
            this.t2_GivenNoStreamWhenEmittingSingleEventThenCanPersistNewStreamWithSingleEvent();
            this.t3_GivenStreamWithSingleEventWhenRetrievingThenRehydratesObject();

            // Multiple event tests
            this.t4_GivenEventSourcedObjectWithNoStreamWhenEmittingMultipleEventsThenCanPersist();
            this.t5_GivenStreamOfMultipleEventsThenCanRehydrate();

            // Page read/write config
            this.t6_GivenTwoPageReadWriteThenCanReadWrite();

            // Snapshotter
            this.t7_SnapshotterIsCalledOnSave();
            this.t8_GivenNoSnapshotAndNoStreamWhenSnapshotterIsCalledOnGetThenGoesToDbButReturnsNull();
            this.t9_GivenSnapshotWhenRetrievingThenInmediatelyReturnsTheInMemoryState();
        }

        public void t1_GivenNoStreamWhenTryingToGetThenReturnsNull()
        {
            var streamName = "simple-aggregate-856987";
            var aggregate = this.sut.GetAsync<ComplexAggregate>(streamName).Result;

            Assert.IsNull(aggregate);
        }

        public void t2_GivenNoStreamWhenEmittingSingleEventThenCanPersistNewStreamWithSingleEvent()
        {
            var aggregate = new ComplexAggregate();
            aggregate.Emit(new NewAgregateCreated("test-1"));

            var task = this.sut.SaveAsync(aggregate);
            task.Wait();

            Assert.IsFalse(task.IsFaulted);
            Assert.IsTrue(task.IsCompleted);
        }

        public void t3_GivenStreamWithSingleEventWhenRetrievingThenRehydratesObject()
        {
            var aggregate = new ComplexAggregate();
            aggregate.Emit(new NewAgregateCreated("test-3"));

            this.sut.SaveAsync(aggregate).Wait();

            aggregate = null;
            aggregate = this.sut.GetAsync<ComplexAggregate>("test-3").Result;

            Assert.IsNotNull(aggregate);
            Assert.AreEqual("test-3", aggregate.StreamName);
            Assert.AreEqual(0, aggregate.Version);
        }

        private void t4_GivenEventSourcedObjectWithNoStreamWhenEmittingMultipleEventsThenCanPersist()
        {
            var aggregate = new ComplexAggregate();
            aggregate.Emit(new NewAgregateCreated("test-4"));
            aggregate.Emit(new OneStuffHappened());
            aggregate.Emit(new TwoStuffHappened());

            var task = this.sut.SaveAsync(aggregate);
            task.Wait();

            Assert.IsFalse(task.IsFaulted);
            Assert.IsTrue(task.IsCompleted);
        }

        private void t5_GivenStreamOfMultipleEventsThenCanRehydrate()
        {
            var aggregate = new ComplexAggregate();
            aggregate.Emit(new NewAgregateCreated("test-5"));
            aggregate.Emit(new OneStuffHappened());
            aggregate.Emit(new TwoStuffHappened());

            var task = this.sut.SaveAsync(aggregate);
            task.Wait();

            aggregate = null;
            aggregate = this.sut.GetAsync<ComplexAggregate>("test-5").Result;

            Assert.IsNotNull(aggregate);
            Assert.AreEqual("test-5", aggregate.StreamName);
            Assert.AreEqual(2, aggregate.Version);
            Assert.AreEqual(3, aggregate.StuffHappenedCount);
        }

        private void t6_GivenTwoPageReadWriteThenCanReadWrite()
        {
            this.sut = new EventSourcedRepository(
                this.esManager.GetFailFastConnection,
                new JsonTextSerializer(),
                this.snapshotter,
                2, 2);

            var aggregate = new ComplexAggregate();
            aggregate.Emit(new NewAgregateCreated("test-6"));
            aggregate.Emit(new OneStuffHappened());
            aggregate.Emit(new TwoStuffHappened());

            var task = this.sut.SaveAsync(aggregate);
            task.Wait();

            aggregate = null;
            aggregate = this.sut.GetAsync<ComplexAggregate>("test-6").Result;

            Assert.IsNotNull(aggregate);
            Assert.AreEqual("test-6", aggregate.StreamName);
            Assert.AreEqual(2, aggregate.Version);
            Assert.AreEqual(3, aggregate.StuffHappenedCount);
        }

        private void t7_SnapshotterIsCalledOnSave()
        {
            var beforeCount = this.snapshotter.ToCacheCallCount;

            var aggregate = new ComplexAggregate();
            aggregate.Emit(new NewAgregateCreated("test-7"));
            aggregate.Emit(new OneStuffHappened());
            aggregate.Emit(new TwoStuffHappened());

            var task = this.sut.SaveAsync(aggregate);
            task.Wait();

            Assert.AreEqual(beforeCount + 1, this.snapshotter.ToCacheCallCount);
        }

        private void t8_GivenNoSnapshotAndNoStreamWhenSnapshotterIsCalledOnGetThenGoesToDbButReturnsNull()
        {
            var beforeCount = this.snapshotter.ToRehydrateCallCount;
            var aggregate = this.sut.GetAsync<ComplexAggregate>("no-snapshot-no-stream").Result;
            Assert.AreEqual(beforeCount + 1, this.snapshotter.ToRehydrateCallCount);
            Assert.IsNull(aggregate);
        }

        private void t9_GivenSnapshotWhenRetrievingThenInmediatelyReturnsTheInMemoryState()
        {
            var beforeCount = this.snapshotter.ToRehydrateCallCount;

            this.snapshotter.SetSnapshot(new ComplexAggregateSnapshot("t9", 10, 11));
            var aggregate = this.sut.GetAsync<ComplexAggregate>("t9").Result;

            Assert.AreEqual(beforeCount + 1, this.snapshotter.ToRehydrateCallCount);
            Assert.IsNotNull(aggregate);
            Assert.AreEqual("t9", aggregate.StreamName);
            Assert.AreEqual(10, aggregate.Version);
            Assert.AreEqual(11, aggregate.StuffHappenedCount);
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