namespace Agrobook.Common
{
    public interface IOneWayEncryptor
    {
        string Encrypt(string text);
    }

    public interface IDecryptor : IOneWayEncryptor
    {
        string Decrypt(string text);
    }
}
