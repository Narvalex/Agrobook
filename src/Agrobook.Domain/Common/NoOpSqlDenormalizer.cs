using Eventing.Core.Messaging;

namespace Agrobook.Domain.Common
{
    public class NoOpSqlDenormalizer : SqlDenormalizer,
        IHandler<object>
    {
        public NoOpSqlDenormalizer(SqlDenormalizerConfig config) : base(config)
        {
        }

        public void Handle(long checkpoint, object e)
        {
            this.Denormalize(checkpoint, c => { });
        }
    }
}
