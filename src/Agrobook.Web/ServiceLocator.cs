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
            var loginClient = new LoginClient("http://localhost:8081", serializer);

            container.Register<LoginClient>(loginClient);
        }
    }
}