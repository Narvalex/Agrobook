using System;
using System.Collections.Generic;

namespace Agrobook.Core
{
    public interface IEventSourced
    {
        string StreamName { get; }
        long Version { get; }
        void Update(object @event);
        void Emit(object @event);
        ICollection<object> NewEvents { get; }
        ISnapshot TakeSnapshot();
    }

    public interface ISnapshot
    {
        string StreamName { get; }
        long Version { get; }
    }

    public class Snapshot : ISnapshot
    {
        public Snapshot(string streamName, long version)
        {
            this.StreamName = streamName;
            this.Version = version;
        }

        public string StreamName { get; }
        public long Version { get; }
    }

    public abstract class EventSourced : IEventSourced
    {
        private readonly ICollection<object> newEvents = new List<object>();
        private readonly IDictionary<Type, Action<object>> handlers = new Dictionary<Type, Action<object>>();

        public ICollection<object> NewEvents => this.newEvents;

        public long Version { get; private set; } = -1;

        public string StreamName { get; protected set; }

        protected void On<T>(Action<T> handler) => this.handlers[typeof(T)] = e => handler((T)e);

        void IEventSourced.Update(object @event)
        {
            var eventType = @event.GetType();
            if (this.handlers.TryGetValue(eventType, out Action<object> handler))
                handler.Invoke(@event);
            // Happily ignoring event handler not found

            this.Version++;
        }

        public void Emit(object @event)
        {
            ((IEventSourced)this).Update(@event);
            this.newEvents.Add(@event);
        }

        public virtual void Rehydrate(ISnapshot snapshot)
        {
            this.StreamName = snapshot.StreamName;
            this.Version = snapshot.Version;
        }

        public virtual ISnapshot TakeSnapshot() => new Snapshot(this.StreamName, this.Version);
    }
}
