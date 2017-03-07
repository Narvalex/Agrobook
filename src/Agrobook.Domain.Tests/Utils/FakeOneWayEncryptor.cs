using Agrobook.Core;

namespace Agrobook.Domain.Tests.Utils
{
    public class FakeOneWayEncryptor : IOneWayEncryptor
    {
        public string Encrypt(string text) => text;
    }
}
