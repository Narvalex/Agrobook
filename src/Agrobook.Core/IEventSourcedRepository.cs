using System;
using System.Threading.Tasks;

namespace Agrobook.Core
{
    public interface IEventSourcedReader
    {
        /// <summary>
        /// Gets the event sourced object, if found. If not found returns null.
        /// </summary>
        Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new();
    }

    public interface IEventSourcedRepository : IEventSourcedReader
    {
        /// <summary>
        /// Saves all new events emmited by the event sourced object. If no new events, then no-op. 
        /// </summary>
        Task SaveAsync(IEventSourced eventSourced);
    }

    public class UniqueConstraintViolationException : Exception
    {
        public UniqueConstraintViolationException(string streamName)
            : base($"The stream {streamName} already exists")
        { }
    }

    public class OptimisticConcurrencyCheckException : Exception
    {
        public OptimisticConcurrencyCheckException(int expectedVersion, int actualVersion)
            : base($"The expected version is {expectedVersion} but the actual version is {actualVersion}")
        { }
    }
}
