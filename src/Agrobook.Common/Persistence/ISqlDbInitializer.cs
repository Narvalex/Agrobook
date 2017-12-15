namespace Agrobook.Common.Persistence
{
    public interface ISqlDbInitializer
    {
        void CreateDatabaseIfNotExists();
        void DropAndCreateDb();
    }
}