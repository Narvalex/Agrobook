using Agrobook.Infrastructure.Cryptography;
using Agrobook.Infrastructure.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Infrastructure.Tests.Serialization
{
    [TestClass]
    public class CryptoSerializerTests
    {
        private CryptoSerializer sut;

        private readonly SimplePoco simplePoco = new SimplePoco("user", 18);

        public CryptoSerializerTests()
        {
            this.sut = new CryptoSerializer(new RijndaelDecryptor());
        }

        [TestMethod]
        public void CanSerializedAndDeserializeSimplePoco()
        {
            var serialized = this.sut.Serialize(this.simplePoco);

            Assert.IsFalse(string.IsNullOrWhiteSpace(serialized));

            var deserialized = this.sut.Deserialize<SimplePoco>(serialized);

            Assert.AreEqual(this.simplePoco.Name, deserialized.Name);
            Assert.AreEqual(this.simplePoco.Age, deserialized.Age);
        }

        public class SimplePoco
        {
            public SimplePoco(string name, int age)
            {
                this.Name = name;
                this.Age = age;
            }

            public string Name { get; }
            public int Age { get; }
        }

    }
}
