﻿using Agrobook.Core;
using System;
using System.Collections.Generic;

namespace Agrobook.Domain.Tests.Utils
{
    public class TestableEventSourcedService<T>
        : IGivenReady<T>, IWhenReady<T>, IThenReady, IAndThenReady where T : class
    {
        private readonly T service;
        private readonly FakeRepo fakeRepo = new FakeRepo();

        public TestableEventSourcedService(Func<IEventSourcedRepository, T> serviceFactory)
        {
            Ensure.NotNull(serviceFactory, nameof(serviceFactory));

            this.service = serviceFactory.Invoke(this.fakeRepo);
        }

        public IWhenReady<T> Given(string streamName = null, params object[] @events)
        {
            this.fakeRepo.PreloadStream(streamName, @events);
            return this;
        }

        public IWhenReady<T> And(string streamName = null, params object[] @events)
        {
            return this.Given(streamName, @events);
        }

        public IThenReady When(Action<T> handling)
        {
            handling.Invoke(this.service);
            return this;
        }

        public IAndThenReady Then(Action<ICollection<object>> assert)
        {
            assert.Invoke(this.fakeRepo.NewEventsCommitted);
            return this;
        }

        public void And<TSnapshot>(Action<TSnapshot> assert) where TSnapshot : ISnapshot
        {
            assert.Invoke((TSnapshot)this.fakeRepo.Snapshot);
        }
    }

    internal class FakeRepo : IEventSourcedRepository
    {
        private readonly IDictionary<string, object[]> eventStore = new Dictionary<string, object[]>();

        internal ICollection<object> NewEventsCommitted { get; private set; }
        internal ISnapshot Snapshot { get; private set; }

        public void PreloadStream(string streamName, params object[] @events)
        {
            if (@events.Length < 1) return;

            this.eventStore[streamName] = @events;
        }

        public T Get<T>(string streamName) where T : class, IEventSourced, new()
        {
            if (!this.eventStore.ContainsKey(streamName))
                return null;

            var stream = this.eventStore[streamName];

            var state = new T();

            for (int i = 0; i < stream.Length; i++)
                state.Update(stream[i]);

            return state;
        }

        public void Save<T>(T updatedState) where T : class, IEventSourced, new()
        {
            this.NewEventsCommitted = updatedState.NewEvents;
            this.Snapshot = updatedState.TakeSnapshot();
        }
    }

    public interface IGivenReady<T>
    {
        IWhenReady<T> Given(string streamName, params object[] @event);
    }

    public interface IWhenReady<T>
    {
        IWhenReady<T> And(string streamName, params object[] @event);
        IThenReady When(Action<T> handling);
    }

    public interface IThenReady
    {
        IAndThenReady Then(Action<ICollection<object>> assert);
    }

    public interface IAndThenReady
    {
        void And<TSnapshot>(Action<TSnapshot> assert) where TSnapshot : ISnapshot;
    }
}
