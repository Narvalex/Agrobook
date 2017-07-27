using Agrobook.Common;
using System.Threading.Tasks;

namespace Agrobook.Domain.Tests.Utils
{
    public class FakeOneWayEncryptor : IOneWayEncryptor
    {
        public string Encrypt(string text) => text;

        public Task<string> EncryptAsync(string text) => Task.FromResult(text);
    }
}
