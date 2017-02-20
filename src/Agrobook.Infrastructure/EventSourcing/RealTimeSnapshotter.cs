using Agrobook.Core;
using System;

namespace Agrobook.Infrastructure.EventSourcing
{
    public class RealTimeSnapshotter : IRealTimeSnapshotter
    {
        public void Cache(ISnapshot snapshot)
        {
            throw new NotImplementedException();
        }

        public bool TryRehydrate<T>(T eventSourced)
        {
            throw new NotImplementedException();
        }
    }
}
