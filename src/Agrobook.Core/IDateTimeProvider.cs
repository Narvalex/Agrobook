using System;

namespace Agrobook.Core
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
