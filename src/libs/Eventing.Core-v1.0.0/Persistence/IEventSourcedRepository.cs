using Eventing.Core.Domain;
using System;
using System.Threading.Tasks;

namespace Eventing.Core.Persistence
{
    public interface IEventSourcedRepository : IEventSourcedReader
    {
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
