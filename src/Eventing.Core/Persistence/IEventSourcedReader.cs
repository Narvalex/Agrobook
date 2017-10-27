using Eventing.Core.Domain;
using System.Threading.Tasks;

namespace Eventing.Core.Messaging
{
    public interface IEventSourcedReader
    {
        // StreamName = StreamCagegory + StreamId
        Task<T> GetAsync<T>(string streamName) where T : class, IEventSourced, new();

        Task<bool> Exists(string streamName);
    }
}
