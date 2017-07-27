using Agrobook.Core;
using Eventing.Core.Domain;

namespace Agrobook.Infrastructure.Tests.EventSourcing.Fakes
{
    public class ComplexAggregate : EventSourced
    {
        public ComplexAggregate()
        {
            this.On<NewAgregateCreated>(e => this.StreamName = e.AggregateId);
            this.On<OneStuffHappened>(e => this.StuffHappenedCount += 1);
            this.On<TwoStuffHappened>(e => this.StuffHappenedCount += 2);
        }

        protected override void Rehydrate(ISnapshot snapshot)
        {
            base.Rehydrate(snapshot);

            var state = (ComplexAggregateSnapshot)snapshot;
            this.StuffHappenedCount = state.StuffHappenedCount;
        }

        protected override ISnapshot TakeSnapshot()
        {
            return new ComplexAggregateSnapshot(this.StreamName, this.Version, this.StuffHappenedCount);
        }

        public int StuffHappenedCount { get; private set; }
    }

    public class ComplexAggregateSnapshot : Snapshot
    {
        public ComplexAggregateSnapshot(string streamName, int version, int stuffHappenedCount) : base(streamName, version)
        {
            this.StuffHappenedCount = stuffHappenedCount;
        }

        public int StuffHappenedCount { get; }
    }

    public class NewAgregateCreated
    {
        public NewAgregateCreated(string aggregateId)
        {
            this.AggregateId = aggregateId;
        }
        public string AggregateId { get; }
    }

    public class OneStuffHappened { }

    public class TwoStuffHappened { }
}
