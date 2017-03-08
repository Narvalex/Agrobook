using Agrobook.Core;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Agrobook.Infrastructure.Cryptography
{
    public class MD5OneWayEncryptor : IOneWayEncryptor
    {
        public string Encrypt(string text)
        {
            // Credits to: https://www.junian.net/2014/07/password-encryption-using-md5-hash.html

            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            return md5.Hash.ToHexString();
        }

        public Task<string> EncryptAsync(string text)
        {
            return Task.FromResult(this.Encrypt(text));
        }
    }
}
