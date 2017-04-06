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
            this.log.Info("Checking the sql databse. If not exists a new one will be created.");
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

        public void DropAndCreateDb()
        {
            this.log.Info("Checking the sql database. If exists it will be deleted and a new one will be created");
            using (var context = this.dbContextFactory.Invoke())
            {
                if (context.Database.Exists())
                {
                    this.log.Info("The sql database was found. Deleting the current db...");
                    context.Database.Delete();
                    this.log.Info("The sql database was successfully deleted. Creating a new one...");
                }

                context.Database.Create();
                this.log.Info("Database created successfully");
            }
        }
    }
}
