namespace Agrobook.Core
{
    /// <summary>
    /// This will help to quickly identify the stream id to which the event belongs
    /// </summary>
    public interface IEvent
    {
        string StreamId { get; }

        /// <summary>
        /// The sequence id, or the version of the stream. This could be useful for 
        /// idempotency reasons. But a better approach could be to use a Guid, and if the 
        /// Guid was already processed, then skip that message.
        /// </summary>
        //string StreamVersion { get; }
    }
}
