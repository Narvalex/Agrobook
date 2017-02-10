using System;

namespace Agrobook.Core
{
    public interface IEventSourcedRepository
    {
        // returns null when not found
        T Get<T>(string streamName) where T : class, IEventSourced, new();

        // throws an exception when concurrency check fails
        void Save<T>(T updatedState) where T : class, IEventSourced, new();
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
