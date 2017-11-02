using Agrobook.Domain.Usuarios;
using Eventing.Core.Persistence;
using System.Threading.Tasks;

namespace Agrobook.Domain.Ap
{
    public static class ApIdProvider
    {
        public async static Task<string> ValidarNuevoIdParaProductor(string idUsuario, IEventSourcedReader reader)
        {
            await reader.EnsureExistenceOf<Usuario>(idUsuario).AndNothingMore();
            return idUsuario;
        }
    }
}
