using Agrobook.Core;
using System;

namespace Agrobook.Infrastructure.EventSourcing
{
    public class EventSourcedRepository : IEventSourcedRepository
    {
        public T Get<T>(string streamName) where T : class, IEventSourced, new()
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T updatedState) where T : class, IEventSourced, new()
        {
            throw new NotImplementedException();
        }
    }
}
