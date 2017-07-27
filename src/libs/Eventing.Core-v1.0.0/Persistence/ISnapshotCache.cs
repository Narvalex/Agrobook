using Eventing.Core.Domain;

namespace Eventing.Core.Persistence
{
    public interface ISnapshotCache
    {
        /// <summary>
        /// Tries to rehydrate an event sourced object from the last snapshot taken. Otherwise returns
        /// the object in its initial state.
        /// </summary>
        bool TryGet(string streamName, out ISnapshot snapshot);

        /// <summary>
        /// Caches the snapshot in memory, fast.
        /// </summary>
        /// <param name="snapshot"></param>
        void Cache(ISnapshot snapshot);
    }
}
