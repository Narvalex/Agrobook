using Agrobook.Core;

namespace Agrobook.Infrastructure.Serialization
{
    public class CryptoSerializer : JsonTextSerializer
    {
        private readonly IDecryptor decryptor;

        public CryptoSerializer(IDecryptor decryptor)
        {
            Ensure.NotNull(decryptor, nameof(decryptor));

            this.decryptor = decryptor;
        }

        public override string Serialize(object value)
        {
            var serialized = base.Serialize(value);
            var encrypted = this.decryptor.Encrypt(serialized);
            return encrypted;
        }

        public override object Deserialize(string value)
        {
            var decrypted = this.decryptor.Decrypt(value);
            return base.Deserialize(decrypted);
        }

        public override T Deserialize<T>(string value)
        {
            var decrypted = this.decryptor.Decrypt(value);
            return base.Deserialize<T>(decrypted);
        }
    }
}
