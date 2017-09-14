using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventing.TestHelpers
{
    public class TestableEventSourcedService<T>
        : IGivenReady<T>, IWhenReady<T>, IThenReady, IAndThenReady where T : class
    {
        private readonly T service;
        private readonly TestableRepository repository = new TestableRepository();

        public TestableEventSourcedService(Func<IEventSourcedRepository, T> serviceFactory)
        {
            Ensure.NotNull(serviceFactory, nameof(serviceFactory));

            this.service = serviceFactory.Invoke(this.repository);
        }

        public IWhenReady<T> Given(string streamName, params object[] @events)
        {
            this.repository.Preload(streamName, @events);
            return this;
        }

        public IWhenReady<T> Given<TState>(string streamId, params object[] @events)
        {
            var streamName = StreamCategoryAttribute.GetFullStreamName<TState>(streamId);
            this.Given(streamName, @events);
            return this;
        }

        public IThenReady When(Action<T> handling)
        {
            handling.Invoke(this.service);
            return this;
        }

        public IAndThenReady Then(Action<ICollection<object>> assert)
        {
            assert.Invoke(this.repository.LastCommited);
            return this;
        }

        public void And<TSnapshot>(Action<TSnapshot> assert) where TSnapshot : ISnapshot
        {
            var snapshot = (TSnapshot)this.repository.LastSnapshot;
            assert.Invoke(snapshot);
        }
    }

    public interface IGivenReady<T>
    {
        IWhenReady<T> Given(string streamName, params object[] @events);
    }

    public interface IWhenReady<T>
    {
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

    public class TestableRepository : InMemoryEventSourcedRepository, IEventSourcedRepository
    {
        private ICollection<string> lastCommited;
        private string lastSnapshot;

        public ICollection<object> LastCommited => this.lastCommited.Select(x => this.serializer.Deserialize(x)).ToArray();
        public ISnapshot LastSnapshot => this.serializer.Deserialize<ISnapshot>(this.lastSnapshot);

        public void Preload(string streamName, object[] @events)
        {
            if (@events.Length < 1) return;

            this.streams[streamName] = @events.Select(x => this.serializer.Serialize(x)).ToList();
        }

        public async new Task SaveAsync(IEventSourced eventSourced)
        {
            // var events = eventSourced.NewEvents; // this reference will die.
            var events = new List<object>();
            events.AddRange(eventSourced.Dehydrate());
            var snapshot = eventSourced.TakeSnapshot();
            await base.SaveAsync(eventSourced);
            this.lastCommited = events.Select(x => this.serializer.Serialize(x)).ToList();
            this.lastSnapshot = this.serializer.Serialize(snapshot);
        }
    }
}


