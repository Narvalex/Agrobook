using Agrobook.Common;
using System;

namespace Agrobook.Common
{
    public class SimpleDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
