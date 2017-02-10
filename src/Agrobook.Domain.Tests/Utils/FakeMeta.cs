using Agrobook.Domain.Common;
using System;

namespace Agrobook.Domain.Tests.Utils
{
    public static class TestMeta
    {
        public static Metadatos New => new Metadatos("Test", DateTime.Now);
    }
}
