namespace Eventing.Core.Messaging
{
    /// <summary>
    /// Marker interface that makes it easier to discover event handlers via reflection
    /// </summary>
    public interface IEventHandler { }

    public interface IHandler<T> : IEventHandler
    {
        void Handle(long checkpoint, T e);
    }
}
