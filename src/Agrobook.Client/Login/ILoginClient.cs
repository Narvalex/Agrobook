using Agrobook.Domain.Usuarios;
using Eventing.Client;
using System.Threading.Tasks;

namespace Agrobook.Client.Login
{
    public interface ILoginClient : ISecuredClient
    {
        Task<LoginResult> TryLoginAsync(string userName, string password);
    }
}