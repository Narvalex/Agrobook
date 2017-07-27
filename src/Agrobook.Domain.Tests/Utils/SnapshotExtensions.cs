using Eventing.Core.Domain;

namespace Agrobook.Domain.Tests
{
    public static class SnapshotExtensions
    {
        public static T Rehydrate<T>(this ISnapshot snapshot) where T : IEventSourced, new()
        {
            var eventSourced = new T();
            eventSourced.Rehydrate(snapshot);
            return eventSourced;
        }
    }
}
