using Agrobook.Core;
using System;
using System.Runtime.Caching;

namespace Agrobook.Infrastructure.EventSourcing
{
    public class SnapshotCache : ISnapshotCache
    {
        private readonly ObjectCache cache;
        private readonly TimeSpan timeToLive;

        public SnapshotCache(ObjectCache cache = null)
            : this(cache, TimeSpan.FromMinutes(30))
        { }

        public SnapshotCache(ObjectCache cache, TimeSpan timeToLive)
        {
            if (cache == null)
                this.cache = new MemoryCache($"RealTimeSnapshotterCache-{Guid.NewGuid()}");
            else
                this.cache = cache;

            this.timeToLive = timeToLive;
        }

        public void Cache(ISnapshot snapshot)
        {
            this.cache.Set(
                key: snapshot.StreamName,
                value: snapshot,
                policy: new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.UtcNow.Add(this.timeToLive) });
        }

        public bool TryGet(string streamName, out ISnapshot snapshot)
        {
            snapshot = (ISnapshot)this.cache.Get(streamName);
            return snapshot != null;
        }
    }
}
