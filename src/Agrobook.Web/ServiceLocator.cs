using Agrobook.Client.Archivos;
using Agrobook.Client.Login;
using Agrobook.Client.Usuarios;
using Agrobook.Common;
using Agrobook.Infrastructure;
using Agrobook.Infrastructure.IoC;
using Eventing.Client.Http;
using Eventing.Core.Serialization;

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

            // Config
            var serverUrl = "http://localhost:8081";

            var serializer = new NewtonsoftJsonSerializer();
            var http = new HttpLite(serverUrl);

            var dateTime = new SimpleDateTimeProvider();

            container.Register<LoginClient>(() => new LoginClient(http));
            container.Register<UsuariosClient>(() => new UsuariosClient(http));
            container.Register<UsuariosQueryClient>(() => new UsuariosQueryClient(http));
            container.Register<ArchivosClient>(() => new ArchivosClient(http));
            container.Register<ArchivosQueryClient>(() => new ArchivosQueryClient(http));
        }
    }
}