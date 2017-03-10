using Agrobook.Infrastructure.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Infrastructure.Tests.Cryptography
{
    [TestClass]
    public class StringCipherTests
    {
        private StringCipher sut = new StringCipher();

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

            Assert.AreNotEqual(encrypted, enc2);
            Assert.AreEqual(decrypted, dec2);
        }
    }
}
