using Agrobook.Core;
using System;

namespace Agrobook.Domain.Tests.Utils
{
    public class TestableEventHandler<T>
    {
        private readonly T handler;

        public TestableEventHandler(Func<IEventStreamSubscriber, T> handlerFactory)
        {
            this.handler = handlerFactory.Invoke(new FakeSubscriber());
        }

        public void When(object e)
        {

        }
    }

    public class FakeSubscriber : IEventStreamSubscriber
    {
        public IEventStreamSubscription CreateSubscription(string streamName, Lazy<long?> lastCheckpoint, Action<long, object> handler)
        {
            throw new NotImplementedException();
        }
    }

    public class FakeSubscription : IEventStreamSubscription
    {
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
