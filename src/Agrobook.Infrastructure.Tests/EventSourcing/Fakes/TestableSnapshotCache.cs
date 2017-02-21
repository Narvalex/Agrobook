using Agrobook.Core;
using Agrobook.Infrastructure.EventSourcing;

namespace Agrobook.Infrastructure.Tests.EventSourcing.Fakes
{
    public class TestableRealTimeSnapshotter : ISnapshotCache
    {
        public int ToCacheCallCount = 0;
        public int ToRehydrateCallCount = 0;
        private ISnapshot snapshot = null;

        public void Cache(ISnapshot snapshot)
        {
            this.ToCacheCallCount += 1;
        }

        public bool TryGet(string streamName, out ISnapshot snapshot)
        {
            this.ToRehydrateCallCount += 1;
            snapshot = this.snapshot;
            return snapshot != null;
        }

        public void SetSnapshot(ISnapshot snapshot)
        {
            this.snapshot = snapshot;
        }
    }
}
