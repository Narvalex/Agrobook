using Eventing.Core.Domain;
using Eventing.Core.Persistence;
using Eventing.Core.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Eventing.EntityFramework.Persistence
{
    public class EfEventStore : IEventSourcedRepository
    {
        private readonly ISnapshotCache snapshotCache;
        private readonly IJsonSerializer serializer;
        private readonly Func<EventStoreDbContext> readDbContextFactory;
        private readonly Func<EventStoreDbContext> writeDbContextFactory;

        private long? lastPosition;
        private readonly object syncRoot = new object();

        public EfEventStore(ISnapshotCache snapshotCache, IJsonSerializer serializer,
            Func<EventStoreDbContext> readDbContextFactory, Func<EventStoreDbContext> writeDbContextFactory)
        {
            Ensure.NotNull(snapshotCache, nameof(snapshotCache));
            Ensure.NotNull(readDbContextFactory, nameof(readDbContextFactory));
            Ensure.NotNull(writeDbContextFactory, nameof(writeDbContextFactory));

            this.snapshotCache = snapshotCache;
            this.serializer = serializer;
            this.readDbContextFactory = readDbContextFactory;
            this.writeDbContextFactory = writeDbContextFactory;

            using (var context = this.readDbContextFactory.Invoke())
            {
                if (context.Database.Exists())
                    this.lastPosition = context.Events.Max(x => x.Position);
            }
        }

        public async Task<bool> Exists(string streamName)
        {
            if (this.snapshotCache.TryGet(streamName, out var snapshot))
                return true;

            using (var context = this.readDbContextFactory.Invoke())
            {
                var streamCategory = GetStreamCategory(streamName);
                var streamId = GetStreamId(streamName);
                return await context.Events.AnyAsync(x =>
                    x.StreamCategory == streamCategory
                    && x.StreamId == streamId);
            }
        }

        public async Task SaveAsync(IEventSourced eventSourced)
        {
            var newEvents = eventSourced.Dehydrate();
            if (newEvents.Count < 1)
                return;

            using (var context = this.writeDbContextFactory.Invoke())
            {
                var commitId = Guid.NewGuid();
                var streamId = GetStreamId(eventSourced.StreamName);
                var timestamp = DateTime.Now;
                var streamCategory = StreamCategoryAttribute.GetCategory(eventSourced.GetType());

                var expectedVersion = eventSourced.Version - newEvents.Count;
                var version = expectedVersion + 1;
                EventDescriptor lastEvent = null;
                newEvents.ForEach(x =>
                {
                    lastEvent = new EventDescriptor
                    {
                        StreamCategory = streamCategory,
                        StreamId = streamId,
                        Version = version,
                        EventType = x.GetType().Name.WithFirstCharInLower(),
                        Payload = this.serializer.Serialize(x),
                        CommitId = commitId,
                        EventId = Guid.NewGuid(),
                        TimeStamp = timestamp
                    };
                    context.Events.Add(lastEvent);

                    version++;
                });

                await context.SaveChangesAsync();

                ThreadPool.QueueUserWorkItem(_ =>
                {
                    lock (this.syncRoot)
                    {
                        if (!this.lastPosition.HasValue || this.lastPosition < lastEvent.Position)
                            this.lastPosition = lastEvent.Position;
                    }
                });
            }

            this.snapshotCache.Cache(eventSourced.TakeSnapshot());
        }

        public async Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new()
        {
            var eventSourced = new T();
            if (this.snapshotCache.TryGet(streamName, out var snapshot))
            {
                eventSourced.Rehydrate(snapshot);
                return eventSourced;
            }

            List<EventDescriptor> events;
            using (var context = this.readDbContextFactory.Invoke())
            {
                var streamCategory = GetStreamCategory(streamName);
                var streamId = GetStreamId(streamName);

                events = await context.Events
                            .Where(x => x.StreamCategory == streamCategory && x.StreamId == streamId)
                            .OrderBy(x => x.Version)
                            .ToListAsync();
            }

            events.ForEach(x =>
            {
                var deserialized = this.serializer.Deserialize(x.Payload);
                eventSourced.Apply(deserialized);
            });

            return eventSourced;
        }


        // Subscription Stuff
        public long? GetCheckpoint(string subscriptionId)
        {
            using (var context = this.readDbContextFactory.Invoke())
            {
                var entity = context.SubscriptionCheckpoints.SingleOrDefault(x => x.SubscriptionId == subscriptionId);
                return entity?.Checkpoint;
            }
        }

        public bool ReceiveEvent(long? position, out EventDescriptor @event)
        {
            if (!(lastPosition > position))
            {
                @event = null;
                return false;
            }

            using (var context = this.readDbContextFactory.Invoke())
            {
                @event = context.Events.Where(x => x.Position > position).OrderBy(x => x.Position).First();
                return true;
            }
        }

        public void SaveCheckpoint(string subscriptionId, long? currentCheckpoint)
        {
            using (var context = this.writeDbContextFactory.Invoke())
            {
                var entity = context.SubscriptionCheckpoints.SingleOrDefault(x => x.SubscriptionId == subscriptionId);
                if (entity == null)
                    entity = new SubscriptionCheckpoint { SubscriptionId = subscriptionId };

                entity.Checkpoint = currentCheckpoint;
                entity.TimeStamp = DateTime.Now;

                context.SaveChanges();
            }
        }

        private static string GetStreamId(string streamName) => streamName.Split('-')[1];
        private static string GetStreamCategory(string streamName) => streamName.Split('-')[0];
    }
}
