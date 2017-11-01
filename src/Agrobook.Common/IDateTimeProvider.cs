using System;

namespace Agrobook.Common
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
