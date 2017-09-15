using System.Collections.Generic;

namespace Eventing.Core.Domain
{
    public interface IEventSourced
    {
        string StreamName { get; }
        int Version { get; }
        void Rehydrate(ISnapshot snapshot);
        void Apply(object @event);
        void Emit(object @event);
        ICollection<object> Peek();
        ICollection<object> Dehydrate();
        ISnapshot TakeSnapshot();
    }
}
