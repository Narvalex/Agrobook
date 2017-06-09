using Agrobook.Client;
using Agrobook.Client.Archivos;
using Agrobook.Client.Login;
using Agrobook.Client.Usuarios;
using Agrobook.Core;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Serialization;

namespace Agrobook.Web
{
    public static class ServiceLocator
    {
        private static ISimpleContainer _container = new SimpleContainer();

        public static T ResolveSingleton<T>() => _container.ResolveSingleton<T>();

        public static T ResolveNewOf<T>() => _container.ResolveNewOf<T>();

        public static void Initialize()
        {
            var container = _container;

            var serializer = new JsonTextSerializer();
            var http = new HttpLite("http://localhost:8081");

            var dateTime = new SimpleDateTimeProvider();

            container.Register<LoginClient>(() => new LoginClient(http));
            container.Register<UsuariosClient>(() => new UsuariosClient(http));
            container.Register<UsuariosQueryClient>(() => new UsuariosQueryClient(http));
            container.Register<ArchivosClient>(() => new ArchivosClient(http));
            container.Register<ArchivosQueryClient>(() => new ArchivosQueryClient(http));
        }
    }
}