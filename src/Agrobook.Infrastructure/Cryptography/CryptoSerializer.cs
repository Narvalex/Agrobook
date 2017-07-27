using Agrobook.Common;
using Eventing;
using Eventing.Core.Serialization;

namespace Agrobook.Infrastructure.Serialization
{
    public class CryptoSerializer : IJsonSerializer
    {
        private readonly IDecryptor decryptor;
        private readonly IJsonSerializer serializer;

        public CryptoSerializer(IDecryptor decryptor, IJsonSerializer serializer)
        {
            Ensure.NotNull(serializer, nameof(serializer));
            Ensure.NotNull(decryptor, nameof(decryptor));

            this.decryptor = decryptor;
            this.serializer = serializer;
        }

        public string Serialize(object value)
        {
            var serialized = this.serializer.Serialize(value);
            var encrypted = this.decryptor.Encrypt(serialized);
            return encrypted;
        }

        public object Deserialize(string value)
        {
            var decrypted = this.decryptor.Decrypt(value);
            return this.serializer.Deserialize(decrypted);
        }

        public T Deserialize<T>(string value)
        {
            var decrypted = this.decryptor.Decrypt(value);
            return this.serializer.Deserialize<T>(decrypted);
        }
    }
}
