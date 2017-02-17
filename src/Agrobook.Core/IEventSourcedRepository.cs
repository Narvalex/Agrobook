using System;
using System.Threading.Tasks;

namespace Agrobook.Core
{
    public interface IEventSourcedRepository
    {
        /// <summary>
        /// Gets the event sourced object, if found.
        /// </summary>
        /// <typeparam name="T">The event sourced type</typeparam>
        /// <param name="streamName">The stream name of the event sourced stream.</param>
        /// <returns>The rehydrated event sourced if found. Otherwise returns null.</returns>
        Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new();

        // throws an exception when concurrency check fails
        Task SaveAsync<T>(T updatedState) where T : class, IEventSourced, new();
    }

    public class UniqueConstraintViolationException : Exception
    {
        public UniqueConstraintViolationException(string streamName)
            : base($"The stream {streamName} already exists")
        {
            this.StreamName = streamName;
        }

        public string StreamName { get; }
    }
}
