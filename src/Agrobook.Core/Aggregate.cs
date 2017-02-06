using System.Collections.Generic;

namespace Agrobook.Core
{
    public abstract class Aggregate
    {
        public void Handle(object message)
        {

        }

        public IEnumerable<object> NewEvents { get; }
    }
}
