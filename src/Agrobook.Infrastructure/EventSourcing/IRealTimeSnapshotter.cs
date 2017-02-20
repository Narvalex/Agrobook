using Agrobook.Core;

namespace Agrobook.Infrastructure.EventSourcing
{
    public interface IRealTimeSnapshotter
    {
        bool TryRehydrate<T>(T eventSourced);
        void Cache(ISnapshot snapshot);
    }
}
