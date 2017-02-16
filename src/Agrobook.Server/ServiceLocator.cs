using Agrobook.Core;
using Agrobook.Infrastructure.IoC;

namespace Agrobook.Server
{
    public static class ServiceLocator
    {
        public static ISimpleContainer Container { get; } = new SimpleContainer();

        public static void Initialize()
        {
            var container = Container;



            //var usuariosYGruposService = new UsuariosYGruposService()
        }
    }
}