namespace Eventing.Core.Messaging
{
    public static class EventProcessorExtensions
    {
        public static void Register(this EventProcessor processor, params IEventHandler[] handlers)
        {
            for (int i = 0; i < handlers.Length; i++)
                processor.Register(handlers[i]);
        }
    }
}
