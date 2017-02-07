using Agrobook.Core;
using System;
using System.Collections.Generic;

namespace Agrobook.Domain.Tests.Utils
{
    public class TestableCommandHandler<T> : IGivenReady<T>, IWhenReady<T>, IThenReady where T : class
    {
        private readonly T service;
        private readonly FakeRepo fakeRepo;

        public TestableCommandHandler(Func<IEventSourcedRepository, T> serviceFactory)
        {
            Ensure.NotNull(serviceFactory, nameof(serviceFactory));

            this.service = serviceFactory.Invoke(this.fakeRepo);
        }

        public IWhenReady<T> Given(params object[] @events)
        {
            this.fakeRepo.Given(@events);
            return this;
        }

        public IThenReady When(Action<T> handling)
        {
            handling.Invoke(this.service);
            return this;
        }

        public ICollection<object> Then()
        {
            return null;
        }
    }

    internal class FakeRepo : IEventSourcedRepository
    {


        public void Given(params object[] @events)
        {

        }

        public T Get<T>(string streamName) where T : IEventSourced => throw new NotImplementedException();
        public void Persist<T>(T updatedState) where T : IEventSourced => throw new NotImplementedException();
    }

    public interface IGivenReady<T>
    {
        IWhenReady<T> Given(params object[] @event);
    }

    public interface IWhenReady<T>
    {
        IThenReady When(Action<T> handling);
    }

    public interface IThenReady
    {
        ICollection<object> Then();
    }
}
