using Agrobook.Core;
using Agrobook.Infrastructure.EventSourcing;

namespace Agrobook.Infrastructure.Tests.EventSourcing.Fakes
{
    public class TestableRealTimeSnapshotter : IRealTimeSnapshotter
    {
        public int ToCacheCallCount = 0;
        public int ToRehydrateCallCount = 0;

        public void Cache(ISnapshot snapshot)
        {
            this.ToCacheCallCount += 1;
        }

        public bool TryRehydrate<T>(T eventSourced)
        {
            this.ToRehydrateCallCount += 1;
            return false;
        }
    }
}
