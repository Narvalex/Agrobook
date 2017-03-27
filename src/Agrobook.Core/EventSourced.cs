using System;
using System.Collections.Generic;

namespace Agrobook.Core
{
    public interface IEventSourced
    {
        string StreamName { get; }
        int Version { get; }
        void Rehydrate(ISnapshot snapshot);
        void Apply(object @event);
        void Emit(object @event);
        ICollection<object> NewEvents { get; }
        void MarkAsCommited();
        ISnapshot TakeSnapshot();
    }

    public interface ISnapshot
    {
        string StreamName { get; }
        int Version { get; }
    }

    public class Snapshot : ISnapshot
    {
        public Snapshot(string streamName, int version)
        {
            this.StreamName = streamName;
            this.Version = version;
        }

        public string StreamName { get; }
        public int Version { get; }
    }

    public abstract class EventSourced : IEventSourced
    {
        private string streamName;

        private readonly ICollection<object> newEvents = new List<object>();
        private readonly IDictionary<Type, Action<object>> handlers = new Dictionary<Type, Action<object>>();

        public int Version { get; private set; } = ExpectedVersion.NoStream; // Empty/NoExistent stream, in Event Store

        public string StreamName
        {
            get => this.streamName;
            protected set => this.streamName = StreamCategoryAttribute.GetFullStreamName(this.GetType(), value);
        }

        ICollection<object> IEventSourced.NewEvents => this.newEvents;

        protected void On<T>(Action<T> handler) => this.handlers[typeof(T)] = e => handler((T)e);

        void IEventSourced.Apply(object @event)
        {
            var eventType = @event.GetType();
            if (this.handlers.TryGetValue(eventType, out Action<object> handler))
                handler.Invoke(@event);
            // Happily ignoring event handler not found

            this.Version++;
        }

        public void Emit(object @event)
        {
            ((IEventSourced)this).Apply(@event);
            this.newEvents.Add(@event);
        }

        void IEventSourced.Rehydrate(ISnapshot snapshot) => this.Rehydrate(snapshot);

        protected virtual void Rehydrate(ISnapshot snapshot)
        {
            this.streamName = snapshot.StreamName;
            this.Version = snapshot.Version;
        }

        ISnapshot IEventSourced.TakeSnapshot() => this.TakeSnapshot();


        protected virtual ISnapshot TakeSnapshot() => new Snapshot(this.StreamName, this.Version);

        void IEventSourced.MarkAsCommited() => this.newEvents.Clear();
    }
}
