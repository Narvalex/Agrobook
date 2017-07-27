using Agrobook.Common;
using System;

namespace Agrobook.Domain.Tests.Utils
{
    public class SimpleDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
