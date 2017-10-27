namespace Agrobook.Core
{
    /// <summary>
    /// This will help to quickly identify the stream id to which the event belongs
    /// </summary>
    public interface IEvent
    {
        string StreamId { get; }
    }
}
