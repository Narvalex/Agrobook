using Agrobook.Infrastructure.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Agrobook.Infrastructure.Tests.Cryptography
{
    [TestClass]
    public class OneWayEncryptorTests
    {
        private MD5OneWayEncryptor sut = new MD5OneWayEncryptor();

        [TestMethod]
        public void CanEncryptSimpleText()
        {
            var text = "pass";
            var encrypted = this.sut.Encrypt(text);

            Console.WriteLine(encrypted);
            Assert.AreNotEqual("pass", encrypted);
        }

        [TestMethod]
        public void ATextEncryptionAlwaysReturnsTheSameEncryptedValue()
        {
            var text = "pass";
            var encrypted = this.sut.Encrypt(text);
            var encryptedAgain = this.sut.Encrypt(text);

            Console.WriteLine(encrypted);
            Assert.AreEqual(encrypted, encryptedAgain);
        }

        [TestMethod]
        public void ATextEncryptionAlwaysReturnsTheSameEncryptedValueAsync()
        {
            var text = "pass";
            var encrypted = this.sut.EncryptAsync(text).Result;
            var encryptedAgain = this.sut.EncryptAsync(text).Result;

            Console.WriteLine(encrypted);
            Assert.AreEqual(encrypted, encryptedAgain);
        }
    }
}
