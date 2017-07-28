using System;
using System.Collections.Generic;

namespace Eventing.Core.Domain
{
    public abstract class EventSourced : IEventSourced
    {
        public const int NoStreamVersionNumber = -1;
        private string streamName;

        private readonly ICollection<object> newEvents = new List<object>();
        private readonly IDictionary<Type, Action<object>> handlers = new Dictionary<Type, Action<object>>();

        public int Version { get; private set; } = NoStreamVersionNumber; // ExpectedVersion.NoStream; // Empty/NoExistent stream, in Event Store

        public string StreamName => this.streamName;

        ICollection<object> IEventSourced.NewEvents => this.newEvents;

        protected void SetStreamNameById(string id)
        {
            this.streamName = StreamCategoryAttribute.GetFullStreamName(this.GetType(), id);
        }

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
