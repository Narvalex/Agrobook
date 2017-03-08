using Agrobook.Infrastructure.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Infrastructure.Tests.Cryptography
{
    [TestClass]
    public class DecryptorTests
    {
        private RijndaelDecryptor sut = new RijndaelDecryptor();

        [TestMethod]
        public void CanEncrypt()
        {
            var text = "pass";
            var encrypted = this.sut.Encrypt(text);

            Assert.AreNotEqual("pass", encrypted);
        }

        [TestMethod]
        public void CanDecryp()
        {
            var text = "pass";
            var encrypted = this.sut.Encrypt(text);
            var decrypted = this.sut.Decrypt(encrypted);

            Assert.AreEqual(text, decrypted);
        }

        [TestMethod]
        public void CanDecrypMultipleTimesTheSameValue()
        {
            var text = "pass";
            var encrypted = this.sut.Encrypt(text);
            var decrypted = this.sut.Decrypt(encrypted);

            Assert.AreEqual(text, decrypted);

            var enc2 = this.sut.Encrypt(text);
            var dec2 = this.sut.Decrypt(encrypted);

            Assert.AreEqual(encrypted, enc2);
            Assert.AreEqual(decrypted, dec2);
        }

        [TestMethod]
        public void CanDecrypMultipleTimesTheSameValueAsync()
        {
            var text = "pass";
            var encrypted = this.sut.EncryptAsync(text).Result;
            var decrypted = this.sut.DecryptAsync(encrypted).Result;

            Assert.AreEqual(text, decrypted);

            var enc2 = this.sut.EncryptAsync(text).Result;
            var dec2 = this.sut.DecryptAsync(encrypted).Result;

            Assert.AreEqual(encrypted, enc2);
            Assert.AreEqual(decrypted, dec2);
        }
    }
}
