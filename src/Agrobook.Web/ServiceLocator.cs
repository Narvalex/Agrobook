using Agrobook.Client;
using Agrobook.Client.Login;
using Agrobook.Core;
using Agrobook.Infrastructure.IoC;
using Agrobook.Infrastructure.Serialization;

namespace Agrobook.Web
{
    public static class ServiceLocator
    {
        public static ISimpleContainer Container { get; } = new SimpleContainer();

        public static void Initialize()
        {
            var container = Container;

            var serializer = new JsonTextSerializer();
            var http = new HttpLite("http://localhost:8081");
            var loginClient = new LoginClient(http);

            container.Register<LoginClient>(loginClient);
        }
    }
}