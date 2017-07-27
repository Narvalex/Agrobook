namespace Eventing.Core.Domain
{
    public interface ISnapshot
    {
        string StreamName { get; }
        int Version { get; }
    }
}
