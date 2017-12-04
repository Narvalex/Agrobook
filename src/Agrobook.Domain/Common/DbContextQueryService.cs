using Eventing;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public abstract class DbContextQueryService<TDbContext> where TDbContext : DbContext
    {
        private readonly Func<TDbContext> contextFactory;

        public DbContextQueryService(Func<TDbContext> contextFactory)
        {
            Ensure.NotNull(contextFactory, nameof(contextFactory));

            this.contextFactory = contextFactory;
        }

        protected async Task<T> QueryAsync<T>(Func<TDbContext, Task<T>> queryExpression)
        {
            using (var context = this.contextFactory.Invoke())
            {
                return await queryExpression(context);
            }
        }
    }
}
