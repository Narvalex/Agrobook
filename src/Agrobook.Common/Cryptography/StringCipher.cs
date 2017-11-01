using Agrobook.Common;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Agrobook.Common.Cryptography
{
    // Thanks to: http://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
    //
    public class StringCipher : IDecryptor
    {
        private const int KeySize = 256;
        private const int DerivationIterations = 2;

        private readonly string password;

        public StringCipher(string password = null)
        {
            this.password = password ?? "trusted";
        }

        public string Encrypt(string text)
        {
            var saltBytes = this.Generate256BitsOfRandomEntropy();
            var ivBytes = this.Generate256BitsOfRandomEntropy();
            var textBytes = Encoding.UTF8.GetBytes(text);
            using (var password = new Rfc2898DeriveBytes(this.password, saltBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(KeySize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(textBytes, 0, textBytes.Length);
                            cryptoStream.FlushFinalBlock();

                            var cipherTextBytes = saltBytes;
                            cipherTextBytes = cipherTextBytes.Concat(ivBytes).ToArray();
                            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                            return cipherTextBytes.ToHexString();
                        }
                    }
                }
            }

        }

        public string Decrypt(string text)
        {
            var completeCipherBytes = text.ToByteArrayFromHexString();
            var saltBytes = completeCipherBytes.Take(KeySize / 8).ToArray();
            var ivBytes = completeCipherBytes.Skip(KeySize / 8).Take(KeySize / 8).ToArray();
            var cipherBytes = completeCipherBytes.Skip((KeySize / 8) * 2).Take(completeCipherBytes.Length - ((KeySize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(this.password, saltBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(KeySize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;

                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivBytes))
                    using (var memoryStream = new MemoryStream(cipherBytes))
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var textBytes = new byte[cipherBytes.Length];
                        var decryptedByteCount = cryptoStream.Read(textBytes, 0, textBytes.Length);
                        return Encoding.UTF8.GetString(textBytes, 0, decryptedByteCount);
                    }
                }
            }
        }

        private byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(randomBytes);
            return randomBytes;
        }
    }
}
