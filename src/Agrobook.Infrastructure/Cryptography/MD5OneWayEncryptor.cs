using System.Security.Cryptography;
using System.Text;
using Agrobook.Core;

namespace Agrobook.Infrastructure.Cryptography
{
    public class MD5OneWayEncryptor : IOneWayEncryptor
    {
        public string Encrypt(string text)
        {
            // Credits to: https://www.junian.net/2014/07/password-encryption-using-md5-hash.html

            var md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.UTF8.GetBytes(text));
            var result = md5.Hash;

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                // change it into 2 hexadecimal digits for each byte
                stringBuilder.Append(result[i].ToString("x2"));

            return stringBuilder.ToString();
        }
    }
}
