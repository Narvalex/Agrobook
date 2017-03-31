using Agrobook.Core;
using Agrobook.Infrastructure.Log;
using System;
using System.Data.Entity;

namespace Agrobook.Infrastructure.Persistence
{
    public class SqlDbInitializer<T> where T : DbContext
    {
        private readonly Func<T> dbContextFactory;
        private readonly ILogLite log = LogManager.GetLoggerFor<SqlDbInitializer<T>>();

        public SqlDbInitializer(Func<T> dbContextFactory)
        {
            Ensure.NotNull(dbContextFactory, nameof(dbContextFactory));

            this.dbContextFactory = dbContextFactory;
        }

        public void CreateDatabaseIfNoExists()
        {
            this.log.Info("Checking the sql databse...");
            using (var context = this.dbContextFactory.Invoke())
            {
                if (!context.Database.Exists())
                {
                    this.log.Info("The sql database was not found. Creating a new one...");
                    context.Database.CreateIfNotExists();
                    this.log.Info("Database created successfully");
                }
                else
                    this.log.Info("Sql database is ready");
            }
        }
    }
}
