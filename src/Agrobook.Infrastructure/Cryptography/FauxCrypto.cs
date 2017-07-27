using Agrobook.Common;
using System.Text;

namespace Agrobook.Infrastructure.Cryptography
{
    public class FauxCrypto : IDecryptor
    {
        public FauxCrypto()
        { }

        public string Decrypt(string text)
        {
            var textBytes = text.ToByteArrayFromHexString();
            return Encoding.UTF8.GetString(textBytes);
        }

        public string Encrypt(string text)
        {
            return Encoding.UTF8.GetBytes(text).ToHexString();
        }
    }
}
