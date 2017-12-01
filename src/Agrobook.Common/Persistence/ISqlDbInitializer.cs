namespace Agrobook.Common.Persistence
{
    public interface ISqlDbInitializer
    {
        void CreateDatabaseIfNoExists();
        void DropAndCreateDb();
    }
}