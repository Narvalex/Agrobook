using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Tests.Utils
{
    public static class TestMeta
    {
        public static Firma New => new Firma("Test", DateTime.Now);
    }
}
