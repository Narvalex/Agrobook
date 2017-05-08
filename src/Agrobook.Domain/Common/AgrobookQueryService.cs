using Agrobook.Core;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public abstract class AgrobookQueryService
    {
        private readonly Func<AgrobookDbContext> contextFactory;

        public AgrobookQueryService(Func<AgrobookDbContext> contextFactory)
        {
            Ensure.NotNull(contextFactory, nameof(contextFactory));

            this.contextFactory = contextFactory;
        }

        protected async Task<T> QueryAsync<T>(Func<AgrobookDbContext, Task<T>> queryExpression)
        {
            using (var context = this.contextFactory.Invoke())
            {
                return await queryExpression(context);
            }
        }
    }
}
