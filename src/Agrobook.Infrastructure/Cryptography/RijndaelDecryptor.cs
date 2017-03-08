using Agrobook.Core;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Agrobook.Infrastructure.Cryptography
{
    public class RijndaelDecryptor : IDecryptor
    {
        private readonly byte[] key;
        private readonly byte[] IV;

        public RijndaelDecryptor(byte[] key = null, byte[] IV = null)
        {
            if (key == null)
                this.key = this.GenerateKey();
            if (IV == null)
                this.IV = this.GenerateIV();
        }

        public string Decrypt(string text)
        {
            var textBytes = text.ToByteArrayFromHexString();

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.Key = this.key;
                rijndael.IV = this.IV;

                using (var decryptor = rijndael.CreateDecryptor(this.key, rijndael.IV))
                using (var msDecrypt = new MemoryStream(textBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }

        public string Encrypt(string text)
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.Key = this.key;
                rijndael.IV = this.IV;

                using (var encryptor = rijndael.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }

                    return msEncrypt.ToArray().ToHexString();
                }
            }
        }

        private byte[] GenerateKey()
        {
            // new byte[] { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf, 0x0 };
            using (var rainDoll = new RijndaelManaged())
            {
                rainDoll.GenerateKey();
                return rainDoll.Key;
            }

        }

        private byte[] GenerateIV()
        {
            using (var rainDoll = new RijndaelManaged())
            {
                rainDoll.GenerateIV();
                return rainDoll.IV;
            }
        }

        public async Task<string> DecryptAsync(string text)
        {
            var textBytes = text.ToByteArrayFromHexString();

            using (var rijndael = new RijndaelManaged())
            {
                rijndael.Key = this.key;
                rijndael.IV = this.IV;

                using (var decryptor = rijndael.CreateDecryptor(this.key, rijndael.IV))
                using (var msDecrypt = new MemoryStream(textBytes))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return await srDecrypt.ReadToEndAsync();
                }
            }
        }

        public async Task<string> EncryptAsync(string text)
        {
            using (var rijndael = new RijndaelManaged())
            {
                rijndael.Key = this.key;
                rijndael.IV = this.IV;

                using (var encryptor = rijndael.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        await swEncrypt.WriteAsync(text);
                    }

                    return msEncrypt.ToArray().ToHexString();
                }
            }
        }
    }
}
