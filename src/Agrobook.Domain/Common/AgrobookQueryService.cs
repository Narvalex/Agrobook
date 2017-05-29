using Agrobook.Core;
using System;
using System.Threading.Tasks;

namespace Agrobook.Domain.Common
{
    public abstract class AgrobookQueryService
    {
        private readonly Func<AgrobookDbContext> contextFactory;
        protected readonly IEventSourcedReader esReader;

        public AgrobookQueryService(Func<AgrobookDbContext> contextFactory, IEventSourcedReader esReader)
        {
            Ensure.NotNull(contextFactory, nameof(contextFactory));
            Ensure.NotNull(esReader, nameof(esReader));

            this.contextFactory = contextFactory;
            this.esReader = esReader;
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
