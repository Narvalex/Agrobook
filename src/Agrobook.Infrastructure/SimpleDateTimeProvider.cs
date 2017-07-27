using Agrobook.Common;
using System;

namespace Agrobook.Infrastructure
{
    public class SimpleDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
