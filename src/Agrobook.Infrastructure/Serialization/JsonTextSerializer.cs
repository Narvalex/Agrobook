using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters;

namespace Agrobook.Infrastructure.Serialization
{
    public class JsonTextSerializer : IJsonSerializer
    {
        private readonly JsonSerializer serializer;

        public JsonTextSerializer()
        {
            this.serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                // Allows deserializing to the actual runtime type
                TypeNameHandling = TypeNameHandling.All,
                // In a version resilient way
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public string Serialize(object value)
        {
            using (var writer = new StringWriter())
            {
                var jsonWriter = new JsonTextWriter(writer);
                this.serializer.Serialize(jsonWriter, value);
                writer.Flush();
                return writer.ToString();
            }
        }

        public object Deserialize(string value)
        {
            using (var reader = new StringReader(value))
            {
                var jsonReader = new JsonTextReader(reader);

                var deserialized = this.serializer.Deserialize(jsonReader);
                return deserialized;
            }
        }

        public T Deserialize<T>(string value)
        {
            using (var reader = new StringReader(value))
            {
                var jsonReader = new JsonTextReader(reader);

                var deserialized = this.serializer.Deserialize<T>(jsonReader);
                return deserialized;
            }
        }
    }
}
