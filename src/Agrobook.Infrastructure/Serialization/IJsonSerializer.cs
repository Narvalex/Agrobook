namespace Agrobook.Infrastructure.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object value);

        object Deserialize(string value);
    }
}
