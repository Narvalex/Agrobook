using System.Threading.Tasks;

namespace Agrobook.Core
{
    public interface IOneWayEncryptor
    {
        string Encrypt(string text);
        Task<string> EncryptAsync(string text);
    }

    public interface IDecryptor : IOneWayEncryptor
    {
        string Decrypt(string text);

        Task<string> DecryptAsync(string text);
    }
}
