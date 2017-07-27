using Eventing.Core.Persistence;
using System;
using System.Threading.Tasks;

namespace Eventing.Core.Domain
{
    public static class EventSourcedReaderExtensions
    {
        public static async Task<T> GetByIdAsync<T>(this IEventSourcedReader reader, string streamId) where T : class, IEventSourced, new()
        {
            var streamName = StreamCategoryAttribute.GetFullStreamName<T>(streamId);
            return await reader.GetAsync<T>(streamName);
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
            var streamName = StreamCategoryAttribute.GetFullStreamName<T>(streamId);
            return await reader.GetOrFailAsync<T>(streamName);
        }
    }
}
