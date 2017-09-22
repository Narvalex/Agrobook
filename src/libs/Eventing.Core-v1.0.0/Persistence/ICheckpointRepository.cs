using System.Threading.Tasks;

namespace Eventing.Core.Persistence
{
    public interface ICheckpointRepository
    {
        long? GetCheckpoint(string subscriptionId);
        Task<long?> GetCheckpointAsync(string subscriptionId);
        void SaveCheckpoint(string subscriptionId, long checkpoint);
        Task SaveCheckpointAsync(string subscriptionId, long checkpoint);
    }
}
