using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization.Formatters;

namespace Agrobook.Infrastructure.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerSettings _settings;

        static JsonSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                // Allows deserializing to the actual runtime type
                TypeNameHandling = TypeNameHandling.All,
                // In a version resilient way
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }

        public object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value, _settings);
        }
    }
}
