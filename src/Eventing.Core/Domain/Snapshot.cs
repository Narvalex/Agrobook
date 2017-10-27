namespace Eventing.Core.Domain
{
    public class Snapshot : ISnapshot
    {
        public Snapshot(string streamName, int version)
        {
            this.StreamName = streamName;
            this.Version = version;
        }

        public string StreamName { get; }
        public int Version { get; }
    }
}
