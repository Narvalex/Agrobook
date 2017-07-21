using Agrobook.Core;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain
{
    public static class EventSourcedReaderExtensions
    {
        public static async Task<T> GetOrFailAsync<T>(this IEventSourcedReader reader, string id)
            where T : class, IEventSourced, new()
        {
            var state = await reader.GetByIdAsync<T>(id);
            if (state is null) throw new InvalidOperationException($"The stream {id} does not exists!");
            return state;
        }
    }
}
