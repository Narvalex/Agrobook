using Eventing;
using Eventing.Core.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Agrobook.Common.Tests.Serialization
{
    [TestClass]
    public class JsonSerializerTests
    {
        private NewtonsoftJsonSerializer sut;

        private readonly SimplePoco simplePoco = new SimplePoco("user", 18);
        private const string _serializedSimplePoco = "{\"$type\":\"Agrobook.Infrastructure.Tests.Serialization.JsonSerializerTests+SimplePoco, Agrobook.Infrastructure.Tests\",\"name\":\"user\",\"age\":18}";

        public JsonSerializerTests()
        {
            this.sut = new NewtonsoftJsonSerializer();
        }

        [TestMethod]
        public void CanSerializeSimplePoco()
        {
            var serialized = this.sut.Serialize(this.simplePoco);

            Ensure.NotNullOrWhiteSpace(serialized, nameof(serialized));

            Assert.AreEqual(_serializedSimplePoco, serialized);
        }

        [TestMethod]
        public void CanDeserializeSimplePoco()
        {
            var deserialized = (SimplePoco)this.sut.Deserialize(_serializedSimplePoco);

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
