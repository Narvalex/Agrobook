namespace Eventing.Core.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object value);

        object Deserialize(string value);

        T Deserialize<T>(string value);
    }
}
