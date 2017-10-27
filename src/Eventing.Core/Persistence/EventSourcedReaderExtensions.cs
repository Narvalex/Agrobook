using Eventing.Core.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eventing.Core.Messaging
{
    public static class EventSourcedReaderExtensions
    {
        public static async Task<T> GetByIdAsync<T>(this IEventSourcedReader reader, string streamId)
            where T : class, IEventSourced, new()
        {
            return await reader.GetAsync<T>(GetStreamName<T>(streamId));
        }

        public static async Task<T> GetOrFailAsync<T>(this IEventSourcedReader reader, string streamName)
           where T : class, IEventSourced, new()
        {
            var state = await reader.GetAsync<T>(streamName);
            if (state is null) throw new InvalidOperationException($"The stream {streamName} does not exists!");
            return state;
        }

        public static async Task<T> GetOrFailByIdAsync<T>(this IEventSourcedReader reader, string streamId)
           where T : class, IEventSourced, new()
        {
            return await reader.GetOrFailAsync<T>(GetStreamName<T>(streamId));
        }

        public static async Task<bool> Exists<T>(this IEventSourcedReader reader, string streamId)
            where T : class, IEventSourced, new()
        {
            return await reader.Exists(GetStreamName<T>(streamId));
        }

        public static async Task EnsureExistence<T>(this IEventSourcedReader reader, string streamId)
            where T : class, IEventSourced, new()
        {
            await reader.EnsureExistence(GetStreamName<T>(streamId));
        }

        public static async Task EnsureExistence(this IEventSourcedReader reader, string streamName)
        {
            if (await reader.Exists(streamName)) return;
            throw new InvalidOperationException($"The stream {streamName} does not exists!");
        }

        public static BulkExistenceChecker EnsureExistenceOf<T>(this IEventSourcedReader reader, string streamId)
            where T : class, IEventSourced, new()
        {
            return new BulkExistenceChecker(reader, GetStreamName<T>(streamId));
        }

        private static string GetStreamName<T>(string streamId)
            where T : class, IEventSourced, new()
            => StreamCategoryAttribute.GetFullStreamName<T>(streamId);

        public class BulkExistenceChecker
        {
            private readonly IEventSourcedReader reader;
            private readonly List<string> streams = new List<string>();

            internal BulkExistenceChecker(IEventSourcedReader reader, string streamId)
            {
                this.reader = reader;
                this.streams.Add(streamId);
            }

            public BulkExistenceChecker And<T>(string streamId)
                where T : class, IEventSourced, new()
            {
                this.streams.Add(GetStreamName<T>(streamId));
                return this;
            }

            public async Task AndNothingMore()
            {
                foreach (var stream in this.streams)
                    await this.reader.EnsureExistence(stream);
            }
        }
    }
}
